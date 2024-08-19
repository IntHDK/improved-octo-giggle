using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebReactApp.Server.Controllers.Identity
{
    [Route("api/identity/google")]
    [ApiController]
    public class IdentityGoogleController : ControllerBase
    {
        private readonly ILogger<IdentityGoogleController> _logger;
        public IdentityGoogleController(ILogger<IdentityGoogleController> logger)
        {
            _logger = logger;
        }
    }
}
