using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebReactApp.Server.ModelObjects;
using WebReactApp.Server.ModelObjects.Identity;
using WebReactApp.Server.Services.AccountService;
using WebReactApp.Server.Services.ItemService;

namespace WebReactApp.Server.Controllers.Items
{
    [Route("items")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly ItemManager _itemmanager;
        private readonly AccountService _accountService;
        public ItemController(ItemManager itemManager, AccountService accountService)
        {
            this._itemmanager = itemManager;
            this._accountService = accountService;
        }

        public class GetItemIndexResponse
        {
            public int Count { get; set; }
            public List<AccountItem> AccountItems { get; set; }
        }
        [Authorize]
        [HttpGet("")]
        public GetItemIndexResponse GetItemIndex()
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    var items = _itemmanager.GetNotExpiredAccountItemsByAccountID(accguid);
                    if (items != null)
                    {
                        return new GetItemIndexResponse { Count = items.Count, AccountItems = items };
                    }
                    else
                    {
                        return new GetItemIndexResponse { Count = 0, AccountItems = [] };
                    }
                }
                catch
                {
                    return new GetItemIndexResponse { Count = 0, AccountItems = [] };
                }
            }
            return new GetItemIndexResponse { Count = 0, AccountItems = [] };
        }

        public class GetItemMyCurrencyResponse
        {
            public int Currency_Cash { get; set; }
            public int Currency_Point { get; set; }
        }
        [Authorize]
        [HttpGet("currency")]
        public GetItemMyCurrencyResponse GetItemMyCurrency()
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    var accinfo = _accountService.GetAccountByAccountID(accguid);
                    if (accinfo != null)
                    {
                        return new GetItemMyCurrencyResponse() {
                            Currency_Cash = accinfo.Currency_Cash,
                            Currency_Point = accinfo.Currency_Point,
                        };
                    }
                    else
                    {
                        return new GetItemMyCurrencyResponse();
                    }
                }
                catch
                {
                    return new GetItemMyCurrencyResponse();
                }
            }
            return new GetItemMyCurrencyResponse();
        }

        //TODO: swagger 자동인식 붙여놨더니 class in class 썼을 때 에러생긴다
        //DB Model을 그대로 붙여썼더니 순환참조로 json 파싱이 불가 주의 (ignore 되어있는지 확인)
        public class GetItemPostIndexResponse
        {
            public List<AccountPost> AccountPosts { get; set; }
        }
        [Authorize]
        [HttpGet("post")]
        public GetItemPostIndexResponse GetItemPostIndex()
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    var postinfo = _itemmanager.GetNotExpiredAccountPostsByAccountID(accguid);
                    if (postinfo != null)
                    {
                        List<AccountPost> accountPosts = postinfo;
                        return new GetItemPostIndexResponse { AccountPosts = accountPosts };
                    }
                    else
                    {
                        return new GetItemPostIndexResponse();
                    }
                }
                catch
                {
                    return new GetItemPostIndexResponse();
                }
            }
            return new GetItemPostIndexResponse();
        }

        public class PostItemPostOpenRequest
        {
            public Guid ID { get; set; }
        }
        public class PostItemPostOpenResponse {
            public bool IsSuccess { get; set; }
        }
        [Authorize]
        [HttpPost("post/open")]
        public PostItemPostOpenResponse PostItemPostOpen(PostItemPostOpenRequest p)
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    var postinfo = _itemmanager.GetPostByPostID(p.ID);
                    if (postinfo != null)
                    {
                        if (postinfo.AccountID != accguid)
                        {
                            return new PostItemPostOpenResponse()
                            {
                                IsSuccess = false,
                            };
                        }
                        return new PostItemPostOpenResponse()
                        {
                            IsSuccess = _itemmanager.MarkAsReadPostByPostID(p.ID),
                        };
                    }
                }
                catch
                {
                    return new PostItemPostOpenResponse()
                    {
                        IsSuccess = false,
                    };
                }
            }
            return new PostItemPostOpenResponse()
            {
                IsSuccess = false,
            };
        }
        public class PostItemPostTakeEnclosureRequest
        {
            public Guid ID { get; set; }
        }
        public class PostItemPostTakeEnclosureResponse
        {
            public bool IsSuccess { get; set; }
        }
        [Authorize]
        [HttpPost("post/takeenclosure")]
        public PostItemPostTakeEnclosureResponse PostItemPostTakeEnclosure(PostItemPostTakeEnclosureRequest p)
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    var postinfo = _itemmanager.GetPostByPostID(p.ID);
                    if (postinfo != null)
                    {
                        if (postinfo.AccountID != accguid)
                        {
                            return new PostItemPostTakeEnclosureResponse()
                            {
                                IsSuccess = false,
                            };
                        }
                        return new PostItemPostTakeEnclosureResponse()
                        {
                            IsSuccess = _itemmanager.TakeEnclosureOnPost(p.ID)
                        };
                    }
                }
                catch
                {
                    return new PostItemPostTakeEnclosureResponse()
                    {
                        IsSuccess = false,
                    };
                }
            }
            return new PostItemPostTakeEnclosureResponse()
            {
                IsSuccess = false,
            };
        }
        public class PostItemPostCleanupRequest
        {
        }
        public class PostItemPostCleanupResponse
        {
            public bool IsSuccess { get; set; }
        }
        [Authorize]
        [HttpPost("post/cleanup")]
        public PostItemPostCleanupResponse PostItemPostCleanup(PostItemPostCleanupRequest p)
        {
            var claimaccountid = User.Claims.Where(c => c.Type == "AccountID").FirstOrDefault();
            if (claimaccountid != null)
            {
                try
                {
                    var accguid = Guid.Parse(claimaccountid.Value);
                    return new PostItemPostCleanupResponse()
                    {
                        IsSuccess = _itemmanager.CleanUpPostWhereIsReadAndIsTakenByAccountID(accguid),
                    };
                }
                catch
                {
                    return new PostItemPostCleanupResponse()
                    {
                        IsSuccess = false,
                    };
                }
            }
            return new PostItemPostCleanupResponse()
            {
                IsSuccess = false,
            };
        }
    }
}
