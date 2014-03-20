namespace DoctrineShips.Repository.Operations
{
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ShipFitOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ShipFitOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteShipFit(int shipFitId)
        {
            this.unitOfWork.Repository<ShipFit>().Delete(shipFitId);
        }

        internal void UpdateShipFit(ShipFit shipFit)
        {
            shipFit.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<ShipFit>().Update(shipFit);
        }

        internal ShipFit AddShipFit(ShipFit shipFit)
        {
            this.unitOfWork.Repository<ShipFit>().Insert(shipFit);
            return shipFit;
        }

        internal ShipFit CreateShipFit(ShipFit shipFit)
        {
            shipFit.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<ShipFit>().Insert(shipFit);
            return shipFit;
        }

        internal ShipFit GetShipFit(int shipFitId)
        {
            return this.unitOfWork.Repository<ShipFit>()
                              .Query()
                              .Include(x => x.ShipFitComponents.Select(c => c.Component))
                              .Get()
                              .FirstOrDefault(x => x.ShipFitId == shipFitId);
                              
        }

        internal IEnumerable<ShipFit> GetShipFits()
        {
            var shipFits = this.unitOfWork.Repository<ShipFit>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.ShipFitId)
                              .ToList();

            return shipFits;
        }

        internal IEnumerable<ShipFit> GetShipFitsWithComponents()
        {
            var shipFits = this.unitOfWork.Repository<ShipFit>()
                              .Query()
                              .Include(x => x.ShipFitComponents.Select(c => c.Component))
                              .Get()
                              .ToList();

            return shipFits;
        }

        internal IEnumerable<ShipFit> GetShipFitsForContractCount()
        {
            var shipFits = this.unitOfWork.Repository<ShipFit>()
                              .Query()
                              .Filter(q => q.ContractsAvailable >= 0)
                              .Get()
                              .ToList();

            return shipFits;
        }

        internal IEnumerable<ShipFit> GetShipFitsForAccount(int accountId)
        {
            var shipFits = this.unitOfWork.Repository<ShipFit>()
                              .Query()
                              .Filter(q => q.AccountId == accountId)
                              .Get()
                              .OrderBy(x => x.ShipFitId)
                              .ToList();

            return shipFits;
        }

        internal Dictionary<string, int> GetShipFitList()
        {
            var shipFitList = this.unitOfWork.Repository<ShipFit>()
                                   .Query()
                                   .Get()
                                   .GroupBy(u => u.FittingHash) // Ignores identical fits in the same account.
                                   .Select(x => new
                                   {
                                       FittingHash = x.FirstOrDefault().FittingHash,
                                       ShipFitId = x.FirstOrDefault().ShipFitId
                                   })
                                   .ToDictionary(x => x.FittingHash, x => x.ShipFitId);

            return shipFitList;
        }
    }
}