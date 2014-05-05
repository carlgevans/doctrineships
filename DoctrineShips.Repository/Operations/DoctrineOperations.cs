namespace DoctrineShips.Repository.Operations
{
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class DoctrineOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal DoctrineOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteDoctrine(int doctrineId)
        {
            this.unitOfWork.Repository<Doctrine>().Delete(doctrineId);
        }

        internal void DeleteDoctrinesByAccountId(int accountId)
        {
            var doctrines = this.unitOfWork.Repository<Doctrine>()
                                   .Query()
                                   .Filter(q => q.AccountId == accountId)
                                   .Get()
                                   .ToList();

            foreach (var doctrine in doctrines)
            {
                this.unitOfWork.Repository<DoctrineShipFit>().Delete(doctrine);
            }
        }

        internal void UpdateDoctrine(Doctrine doctrine)
        {
            doctrine.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Doctrine>().Update(doctrine);
        }

        internal Doctrine AddDoctrine(Doctrine doctrine)
        {
            this.unitOfWork.Repository<Doctrine>().Insert(doctrine);
            return doctrine;
        }

        internal Doctrine CreateDoctrine(Doctrine doctrine)
        {
            doctrine.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Doctrine>().Insert(doctrine);
            return doctrine;
        }

        internal Doctrine GetDoctrine(int doctrineId)
        {
            return this.unitOfWork.Repository<Doctrine>()
                              .Query()
                              .Include(x => x.DoctrineShipFits.Select(c => c.ShipFit))
                              .Get()
                              .FirstOrDefault(x => x.DoctrineId == doctrineId);
        }

        internal IEnumerable<Doctrine> GetDoctrines()
        {
            var doctrines = this.unitOfWork.Repository<Doctrine>()
                            .Query()
                            .Get()
                            .OrderBy(x => x.IsDormant)
                            .ThenByDescending(x => x.IsOfficial)
                            .ThenBy(x => x.Name)
                            .ToList();

            return doctrines;
        }

        internal IEnumerable<Doctrine> GetDoctrinesForAccount(int accountId)
        {
            var doctrines = this.unitOfWork.Repository<Doctrine>()
                            .Query()
                            .Filter(x => x.AccountId == accountId)
                            .Get()
                            .OrderBy(x => x.IsDormant)
                            .ThenByDescending(x => x.IsOfficial)
                            .ThenBy(x => x.Name)
                            .ToList();

            return doctrines;
        }
    }
}