using WebReactApp.Server.ModelObjects.Identity.SessionToken;

namespace WebReactApp.Server.Services.IdentityService
{
    public class IdentityTokenSingleton
    {
        private ILogger<IdentityTokenSingleton> _logger;
        private readonly Dictionary<string, SessionToken> tokens = new Dictionary<string, SessionToken>();

        public IdentityTokenSingleton(ILogger<IdentityTokenSingleton> logger)
        {
            _logger = logger;
        }

        public bool CreateToken(Guid accountid, DateTime expireat,
            out string newtoken, out SessionToken newtokeninfo)
        {
            newtoken = Guid.NewGuid().ToString();
            newtokeninfo = new SessionToken
            {
                AccountID = accountid,
                ExpireAt = expireat,
                Token = newtoken
            };
            return tokens.TryAdd(newtoken, newtokeninfo);
        }
        public bool CheckToken(string token, out SessionToken tokeninfo)
        {
            tokeninfo = null;
            if(tokens.TryGetValue(token, out tokeninfo))
            {
                if (tokeninfo.ExpireAt < DateTime.UtcNow)
                {
                    tokens.Remove(token);
                    tokeninfo = null;
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool VerifyTokenExpiration(string token, DateTime newexpireat)
        {
            if(tokens.TryGetValue(token, out SessionToken tokeninfo))
            {
                if (tokeninfo.ExpireAt < DateTime.UtcNow)
                {
                    tokens.Remove(token);
                    return false;
                }
                tokeninfo.ExpireAt = newexpireat;
                tokens[token] = tokeninfo;
                return true;
            }
            return false;
        }
        public bool DeleteToken(string token)
        {
            tokens.Remove(token);
            return true;
        }
    }
}
