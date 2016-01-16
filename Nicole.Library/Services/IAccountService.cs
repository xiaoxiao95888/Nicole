using System;
using System.Linq;
using Nicole.Library.Models;

namespace Nicole.Library.Services
{
    public interface IAccountService : IDisposable
    {
        void Insert(Account account);
        void Update();
        void Delete(Guid id);
        Account GetAccount(Guid id);
        IQueryable<Account> GetAccounts();
        Account GetAccount(string email);
    }
}
