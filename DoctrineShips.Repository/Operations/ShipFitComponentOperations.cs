namespace DoctrineShips.Repository.Operations
{
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class ShipFitComponentOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ShipFitComponentOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteShipFitComponent(int shipFitComponentId)
        {
            this.unitOfWork.Repository<ShipFitComponent>().Delete(shipFitComponentId);
        }

        internal void DeleteShipFitComponentsByShipFitId(int shipFitId)
        {
            var shipFitComponents = this.unitOfWork.Repository<ShipFitComponent>()
                                   .Query()
                                   .Filter(q => q.ShipFitId == shipFitId)
                                   .Get()
                                   .ToList();

            foreach (var shipFitComponent in shipFitComponents)
            {
                this.unitOfWork.Repository<ShipFitComponent>().Delete(shipFitComponent.ShipFitComponentId);
            }
        }

        internal void UpdateShipFitComponent(ShipFitComponent shipFitComponent)
        {
            shipFitComponent.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<ShipFitComponent>().Update(shipFitComponent);
        }

        internal ShipFitComponent AddShipFitComponent(ShipFitComponent shipFitComponent)
        {
            this.unitOfWork.Repository<ShipFitComponent>().Insert(shipFitComponent);
            return shipFitComponent;
        }

        internal ShipFitComponent CreateShipFitComponent(ShipFitComponent shipFitComponent)
        {
            shipFitComponent.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<ShipFitComponent>().Insert(shipFitComponent);
            return shipFitComponent;
        }

        internal ShipFitComponent GetShipFitComponent(int shipFitComponentId)
        {
            return this.unitOfWork.Repository<ShipFitComponent>().Find(shipFitComponentId);
        }

        internal IEnumerable<ShipFitComponent> GetShipFitComponents(int shipFitId)
        {
            var shipFitComponents = this.unitOfWork.Repository<ShipFitComponent>()
                              .Query()
                              .Filter(q => q.ShipFitId == shipFitId)
                              .Include(x => x.Component)
                              .Get()
                              .ToList();
                              
            return shipFitComponents;
        }
    }
}