using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebReactApp.Server.Data;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.AccountService
{
    public class AccountService
    {
        private readonly AppDbContext appDbContext;
        public AccountService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Account? GetAccountByAccountID(Guid accountID)
        {
            return appDbContext.Accounts.Where(a => a.ID == accountID).FirstOrDefault();
        }

    }
}
