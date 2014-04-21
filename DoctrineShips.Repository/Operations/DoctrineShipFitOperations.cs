namespace DoctrineShips.Repository.Operations
{
    using DoctrineShips.Entities;
    using GenericRepository;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class DoctrineShipFitOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal DoctrineShipFitOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteDoctrineShipFit(int doctrineShipFitId)
        {
            this.unitOfWork.Repository<DoctrineShipFit>().Delete(doctrineShipFitId);
        }

        internal void DeleteDoctrineShipFitsByDoctrineId(int doctrineId)
        {
            var doctrineShipFits = this.unitOfWork.Repository<DoctrineShipFit>()
                                   .Query()
                                   .Filter(q => q.DoctrineId == doctrineId)
                                   .Get()
                                   .ToList();

            foreach (var doctrineShipFit in doctrineShipFits)
            {
                this.unitOfWork.Repository<DoctrineShipFit>().Delete(doctrineShipFit);
            }
        }

        internal void DeleteDoctrineShipFitsByShipFitId(int shipFitId)
        {
            var doctrineShipFits = this.unitOfWork.Repository<DoctrineShipFit>()
                                   .Query()
                                   .Filter(q => q.ShipFitId == shipFitId)
                                   .Get()
                                   .ToList();

            foreach (var doctrineShipFit in doctrineShipFits)
            {
                this.unitOfWork.Repository<DoctrineShipFit>().Delete(doctrineShipFit);
            }
        }

        internal void UpdateDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            doctrineShipFit.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<DoctrineShipFit>().Update(doctrineShipFit);
        }

        internal DoctrineShipFit AddDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            this.unitOfWork.Repository<DoctrineShipFit>().Insert(doctrineShipFit);
            return doctrineShipFit;
        }

        internal DoctrineShipFit CreateDoctrineShipFit(DoctrineShipFit doctrineShipFit)
        {
            doctrineShipFit.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<DoctrineShipFit>().Insert(doctrineShipFit);
            return doctrineShipFit;
        }

        internal DoctrineShipFit GetDoctrineShipFit(int doctrineShipFitId)
        {
            var doctrineShipFit = this.unitOfWork.Repository<DoctrineShipFit>()
                           .Query()
                           .Filter(x => x.DoctrineShipFitId == doctrineShipFitId)
                           .Get()
                           .FirstOrDefault();

            return doctrineShipFit;
        }

        internal IEnumerable<DoctrineShipFit> GetDoctrineShipFits(int doctrineId)
        {
            var doctrineShipFits = this.unitOfWork.Repository<DoctrineShipFit>()
                            .Query()
                            .Filter(x => x.DoctrineId == doctrineId)
                            .Get()
                            .OrderBy(x => x.ShipFitId)
                            .ToList();

            return doctrineShipFits;
        }
    }
}