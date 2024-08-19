using Microsoft.AspNetCore.Authentication;

namespace WebReactApp.Server.Services.Authentication
{
    public class AppAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "AppAuthenticationScheme";
        public string TokenHeaderName { get; set; } = "AppToken";
    }
}
