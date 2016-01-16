using System;
using System.Linq;
using Nicole.Library.Models;
using Nicole.Library.Services;

namespace Nicole.Service.Services
{
     public class AccountService : BaseService, IAccountService
    {
         public AccountService(NicoleDataContext dbContext)
             : base(dbContext)
        {
        }

        public void Insert(Account account)
        {
            this.DbContext.Accounts.Add(account);
            this.DbContext.SaveChanges();
        }

        public void Update()
        {
            this.DbContext.SaveChanges();
        }
        public Account GetAccount(Guid id)
        {
            return this.DbContext.Accounts.FirstOrDefault(u => u.Id == id);
        }

        public Account GetAccount(string email)
        {
            return this.DbContext.Accounts.FirstOrDefault(u => u.Employee.Mail == email);
        }

        public IQueryable<Account> GetAccounts()
        {
            return this.DbContext.Accounts;
        }

        public void Delete(Guid id)
        {
            var account = DbContext.Accounts.FirstOrDefault(n => n.Id == id);
            DbContext.Accounts.Remove(account);
            Update();
        }
    }
}

