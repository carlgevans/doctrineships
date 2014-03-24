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
    /// A class dealing with Doctrine Ships sales agents.
    /// </summary>
    internal sealed class SalesAgentManager
    {
        private readonly IDoctrineShipsRepository doctrineShipsRepository;
        private readonly IEveDataSource eveDataSource;
        private readonly IDoctrineShipsValidation doctrineShipsValidation;
        private readonly ISystemLogger logger;

        /// <summary>
        /// Initialises a new instance of a Sales Agent Manager.
        /// </summary>
        /// <param name="doctrineShipsRepository">An IDoctrineShipsRepository instance.</param>
        /// <param name="eveDataSource">An IEveDataSource instance.</param>
        /// <param name="doctrineShipsValidation">An IDoctrineShipsValidation instance.</param>
        /// <param name="logger">An ISystemLogger logger instance.</param>
        internal SalesAgentManager(IDoctrineShipsRepository doctrineShipsRepository, IEveDataSource eveDataSource, IDoctrineShipsValidation doctrineShipsValidation, ISystemLogger logger)
        {
            this.doctrineShipsRepository = doctrineShipsRepository;
            this.eveDataSource = eveDataSource;
            this.doctrineShipsValidation = doctrineShipsValidation;
            this.logger = logger;
        }

        /// <summary>
        /// Fetches and returns a Doctrine Ships sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which a sales agent object should be returned.</param>
        /// <returns>A sales agents object.</returns>
        internal SalesAgent GetSalesAgent(int salesAgentId)
        {
            return this.doctrineShipsRepository.GetSalesAgent(salesAgentId);
        }

        /// <summary>
        /// Fetches and returns a list of all Doctrine Ships sales agents for a particular account.
        /// </summary>
        /// <param name="accountId">The account for which sales agents should be returned.</param>
        /// <returns>A list of sales agents objects.</returns>
        internal IEnumerable<SalesAgent> GetSalesAgents(int accountId)
        {
            return this.doctrineShipsRepository.GetSalesAgents(accountId);
        }

        /// <summary>
        /// Returns a list of contracts for a given sales agent.
        /// </summary>
        /// <param name="salesAgentId">The id of the sales agent for which contracts should be returned.</param>
        /// <returns>A list of sales agent contract objects.</returns>
        internal IEnumerable<Contract> GetSalesAgentContracts(int salesAgentId)
        {
            return this.doctrineShipsRepository.GetIssuerContracts(salesAgentId);
        }

        /// <summary>
        /// <para>Adds a sales agent from an api key and account id.</para>
        /// </summary>
        /// <param name="apiId">A valid eve api id (keyID).</param>
        /// <param name="apiKey">A valid eve api key (vCode).</param>
        /// <param name="accountId">The id of the account for which a sales agent should be added.</param>
        /// <returns>Returns a validation result object.</returns>
        internal IValidationResult AddSalesAgent(int apiId, string apiKey, int accountId)
        {
            IValidationResult validationResult = new ValidationResult();

            // Fetch details about the Api Key from Eve Data.
            IEveDataApiKey apiKeyInfo = this.eveDataSource.GetApiKeyInfo(apiId, apiKey);

            if (apiKeyInfo != null)
            {
                // Validate the api key.
                validationResult = this.doctrineShipsValidation.ApiKey(apiKeyInfo);
                if (validationResult.IsValid == true)
                {
                    // Use the api key info to populate a new sales agent object.
                    SalesAgent newSalesAgent = new SalesAgent();

                    // If this is a character or account key use the character details, if a corp key use the corp details.
                    if (apiKeyInfo.Type == EveDataApiKeyType.Character || apiKeyInfo.Type == EveDataApiKeyType.Account)
                    {
                        // If this is an account key, the first character in the list will be used.
                        newSalesAgent.SalesAgentId = apiKeyInfo.Characters.FirstOrDefault().CharacterId;
                        newSalesAgent.Name = apiKeyInfo.Characters.FirstOrDefault().CharacterName;
                        newSalesAgent.ImageUrl = eveDataSource.GetCharacterPortraitUrl(newSalesAgent.SalesAgentId);
                        newSalesAgent.IsCorp = false;
                    }
                    else if (apiKeyInfo.Type == EveDataApiKeyType.Corporation)
                    {
                        newSalesAgent.SalesAgentId = apiKeyInfo.Characters.FirstOrDefault().CorporationId;
                        newSalesAgent.Name = apiKeyInfo.Characters.FirstOrDefault().CorporationName;
                        newSalesAgent.ImageUrl = eveDataSource.GetCorporationLogoUrl(newSalesAgent.SalesAgentId);
                        newSalesAgent.IsCorp = true;
                    }

                    // Populate the remaining properties.
                    newSalesAgent.AccountId = accountId;
                    newSalesAgent.ApiId = apiId;
                    newSalesAgent.ApiKey = apiKey;
                    newSalesAgent.IsActive = true;
                    newSalesAgent.LastForce = DateTime.UtcNow;
                    newSalesAgent.LastContractRefresh = DateTime.UtcNow;

                    // Validate the new sales agent.
                    validationResult = this.doctrineShipsValidation.SalesAgent(newSalesAgent);
                    if (validationResult.IsValid == true)
                    {
                        // Add the new sales agent and log the event.
                        this.doctrineShipsRepository.CreateSalesAgent(newSalesAgent);
                        this.doctrineShipsRepository.Save();
                        logger.LogMessage("Sales Agent '" + newSalesAgent.Name + "' Successfully Added For Account Id: " + newSalesAgent.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                }
                else
                {
                    validationResult.AddError("ApiKey.Valid", "An invalid api key was entered or the eve api is currently unavailable.");
                }
            }
            
            return validationResult;
        }

        /// <summary>
        /// <para>Deletes a sales agent from an accountId and a salesAgentId.</para>
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being deleted.</param>
        /// <param name="salesAgent">The Id of the sales agent to be deleted.</param>
        /// <returns>Returns true if the deletion was successful or false if not.</returns>
        internal bool DeleteSalesAgent(int accountId, int salesAgentId)
        {
            SalesAgent salesAgent = this.doctrineShipsRepository.GetSalesAgent(salesAgentId);

            if (salesAgent != null)
            {
                // If the account Id matches the account Id of the sales agent being deleted, continue.
                if (accountId == salesAgent.AccountId)
                {
                    // Delete the sales agent and log the event.
                    this.doctrineShipsRepository.DeleteSalesAgent(salesAgent.SalesAgentId);
                    this.doctrineShipsRepository.Save();
                    logger.LogMessage("Sales Agent '" + salesAgent.Name + "' Successfully Deleted For Account Id: " + salesAgent.AccountId, 1, "Message", MethodBase.GetCurrentMethod().Name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Refresh the number of contracts available for each sales agent.
        /// </summary>
        internal void RefreshSalesAgentContractCounts()
        {
            IEnumerable<SalesAgent> salesAgents;
            salesAgents = this.doctrineShipsRepository.GetSalesAgentsForContractCount();

            if (salesAgents.Any() == true)
            {
                Dictionary<long, int> contracts = this.doctrineShipsRepository.GetContractSalesAgentCounts();
                int quantity;

                foreach (var salesAgent in salesAgents)
                {
                    contracts.TryGetValue(salesAgent.SalesAgentId, out quantity);
                    salesAgent.ContractsAvailable = quantity;

                    // Update the contract count value for the current sales agent in the database.
                    this.doctrineShipsRepository.UpdateSalesAgent(salesAgent);
                }

                // Commit the changes to the database.
                this.doctrineShipsRepository.Save();
            }
        }

        /// <summary>
        /// Updates the state of a sales agent.
        /// </summary>
        /// <param name="accountId">The account Id of the requestor. The account Id should own the sales agent being changed.</param>
        /// <param name="salesAgentId">The id of the sales agent to be changed.</param>
        /// <param name="isActive">The required boolean state.</param>
        /// <returns>Returns true if the change was successful or false if not.</returns>
        internal bool UpdateSalesAgentState(int accountId, int salesAgentId, bool isActive)
        {
            SalesAgent salesAgent = this.doctrineShipsRepository.GetSalesAgent(salesAgentId);

            if (salesAgent != null)
            {
                // If the account Id matches the account Id of the sales agent being changed, continue.
                if (accountId == salesAgent.AccountId)
                {
                    // Change the state of the sales agent and log the event.
                    salesAgent.IsActive = isActive;
                    this.doctrineShipsRepository.UpdateSalesAgent(salesAgent);
                    this.doctrineShipsRepository.Save();

                    if (isActive == true)
                    {
                        logger.LogMessage("Sales Agent '" + salesAgent.Name + "' Successfully Enabled For Account Id: " + salesAgent.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }
                    else
                    {
                        logger.LogMessage("Sales Agent '" + salesAgent.Name + "' Successfully Disabled For Account Id: " + salesAgent.AccountId, 2, "Message", MethodBase.GetCurrentMethod().Name);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
