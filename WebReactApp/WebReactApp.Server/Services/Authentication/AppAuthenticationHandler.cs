using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WebReactApp.Server.Services.IdentityService;

namespace WebReactApp.Server.Services.Authentication
{
    public class AppAuthenticationHandler : AuthenticationHandler<AppAuthenticationOptions>
    {
        private readonly IdentityService.IdentityService identityService;
        public AppAuthenticationHandler
        (IOptionsMonitor<AppAuthenticationOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, IdentityService.IdentityService identityService)
        : base(options, logger, encoder)
        {
            this.identityService = identityService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //check header first
            if (!Request.Headers
                .ContainsKey(Options.TokenHeaderName))
            {
                return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
            }

            //get the header and validate
            string token = Request
                .Headers[Options.TokenHeaderName]!;
            var claims = new List<Claim>();

            if (identityService.CheckAppToken(token, out var accinfo, out _))
            {
                //Claims
                claims.Add(new Claim("AccountID", accinfo.ID.ToString()));
                claims.Add(new Claim("NickName", accinfo.NickName));
            }
            else
            {
                return AuthenticateResult.Fail($"Invalid token.");
            }

            var claimsIdentity = new ClaimsIdentity
                (claims, this.Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal
                (claimsIdentity);

            return AuthenticateResult.Success
                (new AuthenticationTicket(claimsPrincipal,
                this.Scheme.Name));
        }
    }
}
