namespace DoctrineShips.Repository.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DoctrineShips.Entities;
    using GenericRepository;
    using Tools;

    internal sealed class AccountOperations
    {
        private readonly IUnitOfWork unitOfWork;

        internal AccountOperations(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        internal void DeleteAccount(int accountId)
        {
            this.unitOfWork.Repository<Account>().Delete(accountId);
        }

        internal void UpdateAccount(Account account)
        {
            account.ObjectState = ObjectState.Modified;
            this.unitOfWork.Repository<Account>().Update(account);
        }

        internal Account AddAccount(Account account)
        {
            this.unitOfWork.Repository<Account>().Insert(account);
            return account;
        }

        internal Account CreateAccount(Account account)
        {
            account.ObjectState = ObjectState.Added;
            this.unitOfWork.Repository<Account>().Insert(account);
            return account;
        }

        internal Account GetAccount(int accountId)
        {
            var account = this.unitOfWork.Repository<Account>()
               .Query()
               .Filter(x => x.AccountId == accountId)
               .Include(x => x.AccessCodes)
               .Include(x => x.SalesAgents)
               .Get()
               .FirstOrDefault();

            return account;
        }

        internal IEnumerable<Account> GetAccounts()
        {
            var accounts = this.unitOfWork.Repository<Account>()
                              .Query()
                              .Include(x => x.AccessCodes)
                              .Include(x => x.SalesAgents)
                              .Get()
                              .OrderBy(x => x.AccountId)
                              .ToList();

            return accounts;
        }

        internal IEnumerable<Account> GetAccountsForNotifications()
        {
            var accounts = this.unitOfWork.Repository<Account>()
                              .Query()
                              .Filter(x => x.IsActive == true)
                              .Include(x => x.NotificationRecipients)
                              .Include(x => x.SettingProfile)
                              .Include(x => x.ShipFits)
                              .Get()
                              .OrderBy(x => x.AccountId)
                              .ToList();

            return accounts;
        }
    }
}