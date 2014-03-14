namespace DoctrineShips.Repository.Operations
{
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;

    internal sealed class AccessCodeOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal AccessCodeOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteAccessCode(int accessCodeId)
        {
            this.unitOfWork.Repository<AccessCode>().Delete(accessCodeId);
        }

        internal void UpdateAccessCode(AccessCode accessCode)
        {
            accessCode.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<AccessCode>().Update(accessCode);
        }

        internal AccessCode AddAccessCode(AccessCode accessCode)
        {
            this.unitOfWork.Repository<AccessCode>().Insert(accessCode);
            return accessCode;
        }

        internal AccessCode CreateAccessCode(AccessCode accessCode)
        {
            accessCode.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<AccessCode>().Insert(accessCode);
            return accessCode;
        }

        internal AccessCode GetAccessCode(int accessCodeId)
        {
            return this.unitOfWork.Repository<AccessCode>().Find(accessCodeId);
        }

        internal IEnumerable<AccessCode> GetAccessCodes()
        {
            var accessCodes = this.unitOfWork.Repository<AccessCode>()
                              .Query()
                              .Get()
                              .OrderBy(x => x.AccessCodeId)
                              .ToList();

            return accessCodes;
        }

        internal IEnumerable<AccessCode> GetSiteAdminAccessCodes()
        {
            var accessCodes = this.unitOfWork.Repository<AccessCode>()
                              .Query()
                              .Filter(q => q.Role == Role.SiteAdmin)
                              .Get()
                              .OrderBy(x => x.AccessCodeId)
                              .ToList();

            return accessCodes;
        }

        internal IEnumerable<AccessCode> GetAccessCodesForAccount(int accountId)
        {
            var accessCodes = this.unitOfWork.Repository<AccessCode>()
                              .Query()
                              .Filter(q => q.AccountId == accountId)
                              .Get()
                              .OrderBy(x => x.Role)
                              .ToList();

            return accessCodes;
        }
    }
}