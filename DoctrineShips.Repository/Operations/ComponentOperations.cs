namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class ComponentOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal ComponentOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteComponent(int componentId)
        {
            this.unitOfWork.Repository<Component>().Delete(componentId);
        }

        internal void UpdateComponent(Component component)
        {
            component.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Component>().Update(component);
        }

        internal Component AddComponent(Component component)
        {
            this.unitOfWork.Repository<Component>().Insert(component);
            return component;
        }

        internal Component CreateComponent(Component component)
        {
            component.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Component>().Insert(component);
            return component;
        }

        internal Component GetComponent(int componentId)
        {
            return this.unitOfWork.Repository<Component>().Find(componentId);
        }

        internal IEnumerable<Component> GetComponents()
        {
            var components = this.unitOfWork.Repository<Component>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.Name)
                              .ToList();

            return components;
        }
    }
}