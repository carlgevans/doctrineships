namespace DoctrineShips.Repository.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;
    using Tools;

    internal sealed class SalesAgentOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal SalesAgentOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteSalesAgent(int salesAgentId)
        {
            this.unitOfWork.Repository<SalesAgent>().Delete(salesAgentId);
        }

        internal void UpdateSalesAgent(SalesAgent salesAgent)
        {
            salesAgent.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<SalesAgent>().Update(salesAgent);
        }

        internal SalesAgent AddSalesAgent(SalesAgent salesAgent)
        {
            this.unitOfWork.Repository<SalesAgent>().Insert(salesAgent);
            return salesAgent;
        }

        internal SalesAgent CreateSalesAgent(SalesAgent salesAgent)
        {
            salesAgent.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<SalesAgent>().Insert(salesAgent);
            return salesAgent;
        }

        internal SalesAgent GetSalesAgent(int salesAgentId)
        {
            return this.unitOfWork.Repository<SalesAgent>().Find(salesAgentId);
        }

        internal IEnumerable<SalesAgent> GetSalesAgents(int accountId)
        {
            var salesAgents = this.unitOfWork.Repository<SalesAgent>()
                              .Query()
                              .Filter(q => q.AccountId == accountId)
                              .Get()
                              .OrderBy(x => x.Name)
                              .ToList();

            return salesAgents;
        }

        internal IEnumerable<SalesAgent> GetSalesAgentsForContractCount()
        {
            var salesAgents = this.unitOfWork.Repository<SalesAgent>()
                              .Query()
                              .Filter(q => q.IsActive)
                              .Get()
                              .ToList();

            return salesAgents;
        }

        internal IEnumerable<SalesAgent> GetSalesAgentsForRefresh(bool force, int batchSize = 10)
        {
            SqlDateTime timeNow = DateTime.UtcNow;

            if (force == true)
            {
                timeNow = SqlDateTime.MaxValue;
                batchSize = int.MaxValue;
            }

            var salesAgents = this.unitOfWork.Repository<SalesAgent>()
                  .Query()
                  .Filter(x => x.IsActive == true && x.LastContractRefresh <= timeNow.Value)
                  .Get()
                  .OrderBy(x => x.LastContractRefresh)
                  .Take(batchSize)
                  .ToList();

            return salesAgents;
        }

        internal void DeleteStaleSalesAgents(DateTime olderThanDate)
        {
            IEnumerable<SalesAgent> salesAgents = null;

            salesAgents = this.unitOfWork.Repository<SalesAgent>()
                      .Query()
                      .Filter(x => x.LastContractRefresh < olderThanDate && x.IsActive == true)
                      .Get()
                      .ToList();

            foreach (var item in salesAgents)
            {
                this.unitOfWork.Repository<SalesAgent>().Delete(item);
            }
        }
    }
}