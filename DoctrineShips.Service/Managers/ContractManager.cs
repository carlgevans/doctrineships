namespace DoctrineShips.Service.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using DoctrineShips.Entities;
    using DoctrineShips.Repository;
    using DoctrineShips.Validation;
    using EveData;
    using EveData.Entities;
    using Tools;

    /// <summary>
    /// A class dealing with Doctrine Ships contracts.
    /// </summary>
    internal sealed class ContractManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;
        private static IDictionary<string, int> shipFitList;

        /// <summary>
        /// Initialises a new instance of a Contract Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal ContractManager(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Ship Fit List accessor. Populates a dictionary of ship fit hashes when accessed.
        /// </summary>
        internal IDictionary<string, int> ShipFitList
        {
            get
            {
                if (shipFitList == null)
                {
                    shipFitList = this.doctrineShipsRepository.GetShipFitList();
                }

                return shipFitList;
            }

            set 
            {
                shipFitList = value;
            }
        }

        /// <summary>
        /// <para>Refresh the contracts of a batchSize of sales agents with a LastContractRefresh time in the past.</para>
        /// </summary>
        /// <param name="force">If true, indicates that all sales agents should be refreshed, ignoring their NextRefresh values.</param>
        /// <param name="batchSize">The number of sales agents to refresh at a time.</param>
        internal void RefreshContracts(bool force, int batchSize)
        {
            IEnumerable<SalesAgent> salesAgents = new List<SalesAgent>();
            salesAgents = doctrineShipsRepository.GetSalesAgentsForRefresh(force, batchSize);

            if (salesAgents.Any() == true)
            {
                // Reset the ship fit list.
                ShipFitList = null;

                this.RefreshContracts(salesAgents);
            }
        }

        /// <summary>
        /// Refresh the contracts from EveData for a list of sales agents.
        /// </summary>
        /// <param name="salesAgents">A list of sales agent objects.</param>
        internal void RefreshContracts(IEnumerable<SalesAgent> salesAgents)
        {
            foreach (SalesAgent salesAgent in salesAgents)
            {
                this.RefreshContracts(salesAgent, batch: true);
            }
            
            // Save all changes to the database.
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Refresh the contracts of a single sales agent from EveData.
        /// </summary>
        /// <param name="salesAgents">A sales agent object.</param>
        /// <param name="batch">Optional parameter with a default of false. Is this refresh part of a batch? If so, do not commit.</param>
        internal void RefreshContracts(SalesAgent salesAgent, bool batch = false)
        {
            // Fetch the sales agent's contracts from EveData.
            IEnumerable<IEveDataContract> eveDataContracts = this.eveDataSource.GetContracts(salesAgent.ApiId, salesAgent.ApiKey, salesAgent.SalesAgentId, salesAgent.IsCorp);

            if (eveDataContracts != null && eveDataContracts.Any() != false)
            {
                // Fetch a hashset of the current contract ids in the database.
                HashSet<long> existingContractIds = this.doctrineShipsRepository.GetSalesAgentContractIds(salesAgent.SalesAgentId, salesAgent.IsCorp);

                foreach (var eveDataContract in eveDataContracts)
                {
                    // If the sales agent is a character but the current contract is on behalf of a corporation, skip it.
                    // (The contract should be handled and listed under a corporation agent instead).
                    if (salesAgent.IsCorp == false && eveDataContract.ForCorp == true)
                    {
                        continue;
                    }

                    // If the contract is not of type 'ItemExchange' then skip it.
                    if (eveDataContract.Type != EveDataContractType.ItemExchange)
                    {
                        continue;
                    }

                    // Does the contract already exist in the database?
                    if (existingContractIds.Contains(eveDataContract.ContractId))
                    {
                        // If the status of the Eve contract is no longer 'Outstanding', delete it from the database.
                        if (eveDataContract.Status != EveDataContractStatus.Outstanding)
                        {
                            this.doctrineShipsRepository.DeleteContract(eveDataContract.ContractId);
                        }
                    }
                    else
                    {
                        // Only attempt to add a contract if it is currently of status 'Outstanding'.
                        if (eveDataContract.Status == EveDataContractStatus.Outstanding)
                        {
                            this.AddContract(eveDataContract, salesAgent, batch: true);
                        }
                    }
                }

                // Set the agent's last refresh time.
                salesAgent.LastContractRefresh = DateTime.UtcNow;

                // Update the agent's timestamp in the database.
                this.doctrineShipsRepository.UpdateSalesAgent(salesAgent);

                // If this addition is not part of a batch, commit changes to the database.
                if (batch == false)
                {
                    this.doctrineShipsRepository.Save();
                }
            }
        }

        /// <summary>
        /// Add a new contract to the database from an EveDataContract.
        /// </summary>
        /// <param name="eveDataContract">An EveDataContract object.</param>
        /// <param name="salesAgent">A SalesAgent object representing the contract owner.</param>
        /// <param name="batch">Optional parameter with a default of false. Is this addition part of a batch? If so, do not commit.</param>
        internal Contract AddContract(IEveDataContract eveDataContract, SalesAgent salesAgent, bool batch = false)
        {
            Contract newContract = new Contract();

            newContract.ContractId = eveDataContract.ContractId;
            newContract.AssigneeId = eveDataContract.AssigneeId;
            newContract.StartStationId = eveDataContract.StartStationId;
            newContract.Status = Conversion.StringToEnum<ContractStatus>(eveDataContract.Status.ToString(), ContractStatus.Deleted);
            newContract.Type = Conversion.StringToEnum<ContractType>(eveDataContract.Type.ToString(), ContractType.ItemExchange);
            newContract.Availability = Conversion.StringToEnum<ContractAvailability>(eveDataContract.Availability.ToString(), ContractAvailability.Private);
            newContract.Price = eveDataContract.Price;
            newContract.Volume = eveDataContract.Volume;
            newContract.DateIssued = eveDataContract.DateIssued;
            newContract.DateExpired = eveDataContract.DateExpired;
            newContract.IssuerCorpId = eveDataContract.IssuerCorpId;
            newContract.IssuerId = eveDataContract.IssuerId;
            newContract.ForCorp = eveDataContract.ForCorp;
            newContract.IsValid = false;

            // Populate the contract title/description field.
            if (eveDataContract.Title == string.Empty)
            {
                newContract.Title = "None";
            }
            else
            {
                newContract.Title = eveDataContract.Title;
            }

            // Populate the remaining contract details. These calls are cached.
            newContract.SolarSystemId = this.eveDataSource.GetStationSolarSystemId(newContract.StartStationId);
            newContract.SolarSystemName = this.eveDataSource.GetSolarSystemName(newContract.SolarSystemId);
            newContract.StartStationName = this.eveDataSource.GetStationName(newContract.StartStationId);

            // Attempt to match the contract contents to a ship fit id.
            newContract.ShipFitId = this.ContractShipFitMatch(newContract.ContractId, salesAgent);

            // If the ship fit was matched, set the contract as valid.
            if (newContract.ShipFitId != 0)
            {
                newContract.IsValid = true;
            }

            // Validate the new contract.
            if (this.doctrineShipsValidation.Contract(newContract).IsValid == true)
            {
                // Add the populated contract object to the database.
                this.doctrineShipsRepository.CreateContract(newContract);

                // If this addition is not part of a batch, commit changes to the database.
                if (batch == false)
                {
                    this.doctrineShipsRepository.Save();
                }
            }

            return newContract;
        }

        /// <summary>
        /// Attempts to match a contract's contents to a ship fit.
        /// </summary>
        /// <param name="contractId">The id of the contract to be matched.</param>
        /// <param name="salesAgent">A sales agent object representing the contract owner.</param>
        /// <returns>An integer ship fit id or 0 if no match was made.</returns>
        internal int ContractShipFitMatch(long contractId, SalesAgent salesAgent)
        {
            int shipFitId = 0;

            // Fetch the list of items included in the contract.
            IEnumerable<IEveDataContractItem> contractItems = this.eveDataSource.GetContractItems(salesAgent.ApiId, salesAgent.ApiKey, contractId, salesAgent.SalesAgentId, salesAgent.IsCorp);

            if (contractItems != null && contractItems.Any() == true)
            {
                string concatComponents = string.Empty;
                IEnumerable<ShipFitComponent> compressedContractItems = new List<ShipFitComponent>();

                // Compress the contract items components list, removing duplicates but adding the quantities.
                compressedContractItems = contractItems
                    .OrderBy(o => o.TypeId)
                    .GroupBy(u => u.TypeId)
                    .Select(x => new ShipFitComponent()
                    {
                        ComponentId = x.Key,
                        Quantity = x.Sum(p => p.Quantity)
                    });

                // Concatenate all components and their quantities into a single string.
                foreach (var item in compressedContractItems)
                {
                    concatComponents += item.ComponentId + item.Quantity;
                }

                // Generate a hash for the fitting and salt it with the sales agent's account id. This permits identical fits across accounts.
                string fittingHash = Security.GenerateHash(concatComponents, salesAgent.AccountId.ToString());

                // Attempt to find a ship fit with the same components list as the contract.
                if (!ShipFitList.TryGetValue(fittingHash, out shipFitId))
                {
                    shipFitId = 0;
                }
            }

            return shipFitId;
        }

        /// <summary>
        /// Deletes all expired contracts from the database. This may be required if a contract status change has been missed.
        /// </summary>
        internal void DeleteExpiredContracts()
        {
            var expiredCount = this.doctrineShipsRepository.DeleteExpiredContracts();

            if (expiredCount > 0)
            {
                this.logger.LogMessage("Deleted Expired Contracts: " + expiredCount, 2, "Message", MethodBase.GetCurrentMethod().Name);
            }
            
            this.doctrineShipsRepository.Save();
        }

        /// <summary>
        /// Forces a contract refresh for a single sales agent. This operation is only permitted once every 30 minutes.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being refreshed.</param>
        /// <param name="salesAgentId">The id of the sales agent for which a contract refresh is to be forced.</param>
        /// <returns>Returns true if the force was successful or false if not.</returns>
        internal bool ForceContractRefresh(int accountId, int salesAgentId)
        {
            SalesAgent salesAgent = this.doctrineShipsRepository.GetSalesAgent(salesAgentId);

            if (salesAgent != null)
            {
                // If the account Id matches the account Id of the sales agent being refreshed, continue.
                if (accountId == salesAgent.AccountId)
                {
                    // Has 30 minutes elapsed since the last forced refresh?
                    if (Time.HasElapsed(salesAgent.LastForce, TimeSpan.FromMinutes(30)))
                    {
                        // Reset the ship fit list.
                        ShipFitList = null;

                        // Refresh the contracts for the passed sales agent.
                        this.RefreshContracts(salesAgent, batch: true);

                        // Update the LastForce timestamp and save all changes.
                        salesAgent.LastForce = DateTime.UtcNow;
                        this.doctrineShipsRepository.UpdateSalesAgent(salesAgent);
                        this.doctrineShipsRepository.Save();

                        // Log the operation and return true.
                        this.logger.LogMessage("Contract Refresh Forced For Sales Agent '" + salesAgent.Name + "'.", 2, "Message", MethodBase.GetCurrentMethod().Name);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}