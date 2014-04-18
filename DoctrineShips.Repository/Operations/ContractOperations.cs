namespace DoctrineShips.Repository.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;
    using Tools;

    internal sealed class ContractOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ContractOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteContract(long contractId)
        {
            this.unitOfWork.Repository<Contract>().Delete(contractId);
        }

        internal int DeleteExpiredContracts()
        {
            int expiredCount = 0;

            var expiredContracts = this.unitOfWork.Repository<Contract>()
                                   .Query()
                                   .Filter(q => q.DateExpired <= DateTime.UtcNow)
                                   .Get()
                                   .ToList();

            expiredCount = expiredContracts.Count();

            foreach (var contract in expiredContracts)
            {
                this.unitOfWork.Repository<Contract>().Delete(contract.ContractId);
            }

            return expiredCount;
        }

        internal void DeleteContractsByShipFitId(int shipFitId)
        {
            var shipFitContracts = this.unitOfWork.Repository<Contract>()
                                   .Query()
                                   .Filter(q => q.ShipFitId == shipFitId)
                                   .Get()
                                   .ToList();

            foreach (var contract in shipFitContracts)
            {
                this.unitOfWork.Repository<Contract>().Delete(contract.ContractId);
            }
        }

        internal void UpdateContract(Contract contract)
        {
            contract.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Contract>().Update(contract);
        }

        internal Contract AddContract(Contract contract)
        {
            this.unitOfWork.Repository<Contract>().Insert(contract);
            return contract;
        }

        internal Contract CreateContract(Contract contract)
        {
            contract.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Contract>().Insert(contract);
            return contract;
        }

        internal Contract GetContract(long contractId)
        {
            return this.unitOfWork.Repository<Contract>().Find(contractId);
        }

        internal IEnumerable<Contract> GetContracts()
        {
            var contracts = this.unitOfWork.Repository<Contract>()
                            .Query()
                            .Include(x => x.ShipFit)
                            .Get()
                            .ToList();

            return contracts;
        }

        internal IEnumerable<Contract> GetAssigneeContracts(int assigneeId)
        {
            var assigneeContracts = this.unitOfWork.Repository<Contract>()
                                    .Query()
                                    .Filter(q => q.AssigneeId == assigneeId && q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange)
                                    .Include(x => x.ShipFit)
                                    .Get()
                                    .ToList();

            return assigneeContracts;
        }

        internal IEnumerable<Contract> GetIssuerContracts(int salesAgentId)
        {
            var issuerContracts = this.unitOfWork.Repository<Contract>()
                                    .Query()
                                    .Filter(q => q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange)
                                    .Filter(q => q.IssuerId == salesAgentId && q.ForCorp == false || q.IssuerCorpId == salesAgentId && q.ForCorp == true)
                                    .Include(x => x.ShipFit)
                                    .Get()
                                    .ToList();

            return issuerContracts;
        }

        internal IEnumerable<Contract> GetShipFitContracts(int shipFitId)
        {
            var shipFitContracts = this.unitOfWork.Repository<Contract>()
                        .Query()
                        .Filter(q => q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange)
                        .Filter(q => q.ShipFitId == shipFitId)
                        .Include(x => x.ShipFit)
                        .Get()
                        .ToList();

            return shipFitContracts;
        }

        internal HashSet<long> GetSalesAgentContractIds(int salesAgentId, bool isCorp = false)
        {
            HashSet<long> salesAgentContractIds;

            if (isCorp == false)
            {
                salesAgentContractIds = this.unitOfWork.Repository<Contract>()
                                        .Query()
                                        .Filter(q => q.IssuerId == salesAgentId && q.ForCorp == false)
                                        .Get()
                                        .Select(x => x.ContractId)
                                        .ToHashSet();
            }
            else
            {
                salesAgentContractIds = this.unitOfWork.Repository<Contract>()
                                        .Query()
                                        .Filter(q => q.IssuerCorpId == salesAgentId && q.ForCorp == true)
                                        .Get()
                                        .Select(x => x.ContractId)
                                        .ToHashSet();
            }

            return salesAgentContractIds;
        }

        internal Dictionary<int, int> GetContractShipFitCounts()
        {
            var shipFitContracts = this.unitOfWork.Repository<Contract>()
                                   .Query()
                                   .Filter(q => q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange && q.IsValid == true)
                                   .Get()
                                   .GroupBy(u => u.ShipFitId)
                                   .Select(x => new
                                            {
                                                Key = x.FirstOrDefault().ShipFitId,
                                                Value = x.Count()
                                            })
                                            .ToDictionary(x => x.Key, x => x.Value);

            return shipFitContracts;
        }

        internal Dictionary<long, int> GetContractSalesAgentCounts()
        {
            // Create a dictionary of character sales agent contracts.
            var charContracts = this.unitOfWork.Repository<Contract>()
                               .Query()
                               .Filter(q => q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange && q.ForCorp == false)
                               .Get()
                               .GroupBy(u => u.IssuerId)
                               .Select(x => new
                                        {
                                            Key = x.FirstOrDefault().IssuerId,
                                            Value = x.Count()
                                        })
                                        .ToDictionary(x => x.Key, x => x.Value);

            // Create a dictionary of corporation sales agent contracts.
            var corpContracts = this.unitOfWork.Repository<Contract>()
                               .Query()
                               .Filter(q => q.Status == ContractStatus.Outstanding && q.Type == ContractType.ItemExchange && q.ForCorp == true)
                               .Get()
                               .GroupBy(u => u.IssuerCorpId)
                               .Select(x => new
                                       {
                                           Key = x.FirstOrDefault().IssuerCorpId,
                                           Value = x.Count()
                                       })
                                       .ToDictionary(x => x.Key, x => x.Value);

            // Merge the two dictionaries.
            var salesAgentContracts = charContracts
                                      .Concat(corpContracts)
                                      .GroupBy(d => d.Key)
                                      .ToDictionary(d => d.Key, d => d.First().Value);

            return salesAgentContracts;
        }
    }
}