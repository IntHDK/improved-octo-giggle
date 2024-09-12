using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using WebReactApp.Server.Data;
using WebReactApp.Server.ModelObjects.Identity;
using WebReactApp.Server.ModelObjects.Identity.LoginMethod;
using WebReactApp.Server.ModelObjects.Identity.SessionToken;

namespace WebReactApp.Server.Services.IdentityService
{
    public class IdentityService
    {
#if DEBUG
        public readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(5);
#else
        public readonly TimeSpan TokenLifetime = TimeSpan.FromDays(1);
#endif


        private readonly ILogger<IdentityService> logger;
        private readonly AppDbContext appDbContext;
        private readonly IdentityTokenSingleton tokenSingleton;

        public IdentityService(
            ILogger<IdentityService> logger,
            AppDbContext appDbContext,
            IdentityTokenSingleton tokenSingleton)
        {
            this.logger = logger;
            this.appDbContext = appDbContext;
            this.tokenSingleton = tokenSingleton;
        }

        public bool IsNeedInitialize { get
            {
                if (appDbContext != null)
                {
                    if (appDbContext.AccountRoles.Where(r => r.Role == RoleType.Master).Count() > 0)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            } }
        public bool InitializeMasterAccount(string username, string email, string password)
        {
            if (IsNeedInitialize)
            {
                if (CreateAccount(username, email, out var accinfo))
                {
                    if (AddLoginMethodUsernamePassword(accinfo.AccountID, username, password))
                    {
                        if (AddRole(accinfo.AccountID, new List<RoleType>() { RoleType.Master }))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsThereAccountWithEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            return appDbContext.Accounts.Where(a => a.Email == email).Count() > 0;
        }
        public bool CreateAccount(string nickname, string email, out IdentityManagerAccountInformation accountInformation)
        {            
            accountInformation = new IdentityManagerAccountInformation();
            if (appDbContext != null)
            {
                Account newaccount = new Account
                {
                    CreatedTime = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    NickName = nickname,
                    Email = email,
                    IsConfirmed = false,
                    Roles = []
                };
                try
                {
                    appDbContext.Accounts.Add(newaccount);
                    appDbContext.SaveChanges();
                }
                catch
                {
                    return false;
                }
                accountInformation.AccountID = newaccount.ID;
                accountInformation.NickName = newaccount.NickName;
                accountInformation.RoleTags = new List<RoleType>();

                return true;
            }
            return false;
        }
        public bool RemoveAccount(Guid AccountID)
        {
            if (appDbContext != null)
            {
                try
                {
                    Account account = appDbContext.Accounts.Find(AccountID);
                    if (account != null) {
                        appDbContext.Accounts.Remove(account);
                        appDbContext.SaveChanges();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public bool MakeAccountConfirmationTicket(Guid accountID, DateTime ExpireAt, out AccountConfirmTicket ticket)
        {
            ticket = null;
            //TODO : 이메일로 계정 생성 확인 기능 추가시
            return false;
        }
        public bool ConfirmAccountByTicket(Guid ticketid)
        {
            //TODO : 이메일로 계정 생성 확인 기능 추가시
            return false;
        }
        public bool ConfirmAccountManual(Guid accountID, bool confirmStatus)
        {
            if (appDbContext != null)
            {
                Account? currentaccount = appDbContext.Accounts.Find(accountID);
                if (currentaccount != null)
                {
                    currentaccount.IsConfirmed = confirmStatus;
                    return true;
                }
            }
            return false;
        }
        //username 사용자가 이미 등록되어있는지 체크 (중복체크)
        public bool CheckIsExistUsernameLoginMethodUsernamePassword(string username)
        {
            if (appDbContext != null)
            {
                return appDbContext.UsernamePasswordMethods.Where(m => m.UserName == username).Any();
            }
            else
            {
                return true;
            }
        }
        //username, password로 로그인 가능한 IDPW 로그인 정보를 추가
        public bool AddLoginMethodUsernamePassword(Guid accountID, string username, string password)
        {
            if (appDbContext != null)
            {
                try
                {
                    var curacc = appDbContext.Accounts.Include(a => a.UsernamePasswordMethod).Where(a => a.ID.Equals(accountID)).FirstOrDefault();
                    if (curacc != null)
                    {
                        if (IdentityPasswordCrypt.HashPassword(password,
                            out PasswordHashMethod hashmethod,
                            out byte[] hashedpassword,
                            out int itr,
                            out byte[] passwordsalt,
                            out int saltsize,
                            out KeyDerivationPrf prf)){

                            if (curacc.UsernamePasswordMethod == null)
                            {
                                curacc.UsernamePasswordMethod = new UsernamePasswordMethod
                                {
                                    Account = curacc,
                                    AccountID = curacc.ID,
                                    CreatedTime = DateTime.Now,
                                    PasswordHash = hashedpassword,
                                    PasswordItr = itr,
                                    PasswordMethod = hashmethod,
                                    PasswordPrf = prf,
                                    PasswordSalt = passwordsalt,
                                    PasswordSaltLength = saltsize,
                                    UserName = username
                                };
                            }
                            else
                            {
                                curacc.UsernamePasswordMethod.CreatedTime = DateTime.UtcNow;
                                curacc.UsernamePasswordMethod.UserName = username;
                                curacc.UsernamePasswordMethod.PasswordMethod = hashmethod;
                                curacc.UsernamePasswordMethod.PasswordSalt = passwordsalt;
                                curacc.UsernamePasswordMethod.PasswordSaltLength = saltsize;
                                curacc.UsernamePasswordMethod.PasswordPrf = prf;
                                curacc.UsernamePasswordMethod.PasswordHash = hashedpassword;
                                curacc.UsernamePasswordMethod.PasswordItr = itr;
                            }
                                
                            appDbContext.SaveChanges();
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        //username, password로 패스워드 검증
        public bool LoginWithMethodUsernamePassword(string username, string password,
            out Account account, out SessionToken sessiontoken)
        {
            account = null;
            sessiontoken = null;
            if (appDbContext != null)
            {
                var method = appDbContext.UsernamePasswordMethods.Include(u => u.Account).Where(u => u.UserName == username).FirstOrDefault();
                if (method != null)
                {
                    if (method.Account != null)
                    {
                        account = method.Account;

                        //TODO: 계정 제한 등등 여기서 처리

                        if (IdentityPasswordCrypt.VerifyPassword(password,
                        method.PasswordHash, method.PasswordMethod, method.PasswordItr,
                        method.PasswordSalt, method.PasswordSaltLength, method.PasswordPrf))
                        {
                            //토큰 생성
                            if (tokenSingleton.CreateToken(method.AccountID, DateTime.UtcNow.Add(TokenLifetime),
                                out _, out sessiontoken))
                            {
                                account.LastLoginTime = DateTime.Now;
                                appDbContext.SaveChanges();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //사용자 권한 추가
        //TODO: 중복되는 권한은 추가 안되도록 제어
        public bool AddRole(Guid accountID, List<RoleType> roles)
        {
            if (appDbContext != null)
            {
                var curacc = appDbContext.Accounts.Include(a => a.Roles).Where(a => a.ID == accountID).FirstOrDefault();
                if (curacc != null)
                {
                    foreach (var role in roles)
                    {
                        curacc.Roles.Add(new AccountRole
                        {
                            CreatedTime = DateTime.Now,
                            Role = role
                        });
                    }
                    appDbContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        //토큰 체크 로직 (컨트롤러에서 직접 쓰는거보다 미들웨어에서)
        public bool CheckAppToken(string appToken, out Account account, out SessionToken sessiontoken)
        {
            account = null;
            sessiontoken = null;
            if (appDbContext != null && tokenSingleton != null)
            {
                if (tokenSingleton.CheckToken(appToken, out sessiontoken))
                {
                    var accid = sessiontoken.AccountID;
                    account = appDbContext.Accounts.Find(sessiontoken.AccountID);
                    if (account != null)
                    {
                        DateTime newexp = DateTime.UtcNow.Add(TokenLifetime);
                        tokenSingleton.VerifyTokenExpiration(appToken, newexp);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
