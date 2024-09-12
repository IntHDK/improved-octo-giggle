using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //DB Model을 그대로 붙여썼더니 순환참조로 json 파싱이 불가 주의
        public class GetItemPostIndexResponse_AccountPost
        {
            public Guid ID { get; set; }
            public DateTime CreatedTime { get; set; }
            public string Context { get; set; }
            public DateTime ExpireAt { get; set; }
            public List<GetItemPostIndexResponse_AccountPostEnclosure> Enclosures { get; set; }
            public bool IsRead { get; set; }
            public bool IsEnclosureTaken { get; set; }
        }
        public class GetItemPostIndexResponse_AccountPostEnclosure
        {
            public Guid ID { get; set; }
            public DateTime CreatedTime { get; set; }
            public AccountItemType Type { get; set; }
            public string ItemMetaName { get; set; }
            public DateTime ExpireAt { get; set; }
            public int Quantity { get; set; }
            public List<GetItemPostIndexResponse_AccountPostEnclosureItemParameter> Parameters { get; set; }
        }
        public class GetItemPostIndexResponse_AccountPostEnclosureItemParameter
        {
            public Guid ID { get; set; }
            public string ParamName { get; set; }
            public int Index { get; set; }
            public int NumberValue { get; set; }
            public string StringValue { get; set; }
        }
        public class GetItemPostIndexResponse
        {
            public List<GetItemPostIndexResponse_AccountPost> AccountPosts { get; set; }
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
                        List<GetItemPostIndexResponse_AccountPost> accountPosts = [];
                        postinfo.ForEach((p) =>
                        {
                            var enclosures = new List<GetItemPostIndexResponse_AccountPostEnclosure>();
                            if (p.AccountPostenclosure != null)
                            {
                                p.AccountPostenclosure.ForEach((e) =>
                                {
                                    var parameters = new List<GetItemPostIndexResponse_AccountPostEnclosureItemParameter>();
                                    if (e.Parameters != null)
                                    {
                                        e.Parameters.ForEach((p) =>
                                        {
                                            parameters.Add(new GetItemPostIndexResponse_AccountPostEnclosureItemParameter()
                                            {
                                                ID = p.ID,
                                                Index = p.Index,
                                                NumberValue = p.NumberValue,
                                                ParamName = p.ParamName,
                                                StringValue = p.StringValue,
                                            });
                                        });
                                    }
                                    enclosures.Add(new GetItemPostIndexResponse_AccountPostEnclosure()
                                    {
                                        CreatedTime = e.CreatedTime,
                                        ExpireAt = e.ExpireAt,
                                        ID = e.ID,
                                        ItemMetaName = e.ItemMetaName,
                                        Parameters = parameters,
                                        Quantity = e.Quantity,
                                        Type = e.Type,
                                    });
                                });
                            }
                            accountPosts.Add(new GetItemPostIndexResponse_AccountPost
                            {
                                ID = p.ID,
                                Context = p.Context,
                                ExpireAt = p.ExpireAt,
                                CreatedTime = p.CreatedTime,
                                Enclosures = enclosures,
                                IsRead = p.IsRead,
                                IsEnclosureTaken = p.IsEnclosureTaken,
                            });
                        });
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
    }
}
