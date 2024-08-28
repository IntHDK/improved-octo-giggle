using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebReactApp.Server.ModelObjects.Identity;
using WebReactApp.Server.ModelObjects.Identity.SessionToken;
using WebReactApp.Server.Services.IdentityService;

namespace WebReactApp.Server.Controllers.Identity
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService identityService;
        private readonly ILogger<IdentityController> logger;

        public IdentityController(IdentityService identityService, ILogger<IdentityController> logger)
        {
            this.logger = logger;
            this.identityService = identityService;
        }

        public class PostRegisterIDPWRequest
        {
            [Required] public string Username { get; set; }
            [Required] public string Nickname { get; set; }
            [Required] public string Email { get; set; }
            [Required] public string Password { get; set; }
        }
        public class PostRegisterIDPWResponse
        {
            public bool IsSuccess { get; set; }
        }
        [HttpPost("register/idpw")]
        public PostRegisterIDPWResponse PostRegisterIDPW(PostRegisterIDPWRequest request)
        {
            if (request != null && identityService != null)
            {
                if (identityService.CreateAccount(request.Nickname, request.Email, out var newaccinfo))
                {
                    Console.WriteLine(string.Format("createaccount true: {0}", newaccinfo.NickName));
#if DEBUG
                    if (identityService.ConfirmAccountManual(newaccinfo.AccountID, true))
                    {
                        Console.WriteLine(string.Format("confirmaccountmanual true: {0}", newaccinfo.NickName));
                        if (identityService.AddLoginMethodUsernamePassword(newaccinfo.AccountID, request.Username, request.Password))
                        {
                            return new PostRegisterIDPWResponse
                            {
                                IsSuccess = true
                            };
                        }
                        else
                        {
                            identityService.RemoveAccount(newaccinfo.AccountID); //생성롤백
                            Console.WriteLine(string.Format("addmethod false: {0}", newaccinfo.NickName));
                        }
                    }
#else
                    //TODO: 개발환경이 아닐 때 : 확정이메일 보내기
#endif
                }
            }
            return new PostRegisterIDPWResponse
            {
                IsSuccess = false
            };
        }
        public class GetRegisterIDPWIsAvailableUsernameResponse
        {
            public bool IsAvailable { get; set; }
        }
        [HttpGet("register/idpw/isavailableusername/{username}")]
        public GetRegisterIDPWIsAvailableUsernameResponse GetRegisterIDPWIsAvailableUsername(string username)
        {
            return new GetRegisterIDPWIsAvailableUsernameResponse()
            {
                IsAvailable = !identityService.CheckIsExistUsernameLoginMethodUsernamePassword(username)
            };
        }

        public class PostLoginIDPWRequest
        {
            [Required] public string Username { get; set; }
            [Required] public string Password { get; set; }
        }
        public class PostLoginIDPWResponse
        {
            public bool IsSuccess { get; set; }
            public string Token { get; set; }
            public DateTime ExpireAt { get; set; }
        }
        [HttpPost("login/idpw")]
        public PostLoginIDPWResponse PostLoginIDPW(PostLoginIDPWRequest request)
        {
            if (Request != null && identityService != null)
            {
                if (identityService.LoginWithMethodUsernamePassword(request.Username, request.Password,
                    out Account account, out SessionToken sessiontoken))
                {
                    logger.LogInformation("Login: new token acquired - {0}", sessiontoken.Token);
                    return new PostLoginIDPWResponse
                    {
                        IsSuccess = true,
                        ExpireAt = sessiontoken.ExpireAt,
                        Token = sessiontoken.Token
                    };
                }
            }
            return new PostLoginIDPWResponse
            {
                IsSuccess = false
            };
        }

        public class GetIndexResponse
        {
            public string AccountID { get; set; }
            public string Username { get; set; }
        }
        [Authorize]
        [HttpGet("")]
        public GetIndexResponse GetIndex()
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            var claimnickname = User.Claims.Where(c => c.Type == "NickName").FirstOrDefault();
            return new GetIndexResponse
            {
                AccountID = claimaccountid?.Value ?? "",
                Username = claimnickname?.Value ?? ""
            };
        }
    }
}
