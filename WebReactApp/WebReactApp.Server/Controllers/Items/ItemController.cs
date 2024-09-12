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

        public class GetItemMyPostListResponse_AccountPost
        {
            public Guid ID { get; set; }
            public DateTime CreatedTime { get; set; }
            public string Context { get; set; }
            public DateTime ExpireAt { get; set; }
            public List<GetItemMyPostListResponse_AccountPostEnclosure> Enclosures { get; set; }
            public bool IsRead { get; set; }
            public bool IsEnclosureTaken { get; set; }
        }
        public class GetItemMyPostListResponse_AccountPostEnclosure
        {
            public Guid ID { get; set; }
            public DateTime CreatedTime { get; set; }
            public AccountItemType Type { get; set; }
            public string ItemMetaName { get; set; }
            public DateTime ExpireAt { get; set; }
            public int Quantity { get; set; }
            public List<GetItemMyPostListResponse_AccountPostEnclosureItemParameter> Parameters { get; set; }
        }
        public class GetItemMyPostListResponse_AccountPostEnclosureItemParameter
        {
            public Guid ID { get; set; }
            public string ParamName { get; set; }
            public int Index { get; set; }
            public int NumberValue { get; set; }
            public string StringValue { get; set; }
        }
        public class GetItemMyPostListResponse
        {
            public List<GetItemMyPostListResponse_AccountPost> AccountPosts { get; set; }
        }
        [Authorize]
        [HttpGet("post")]
        public GetItemMyPostListResponse GetItemMyPostList()
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
                        List<GetItemMyPostListResponse_AccountPost> accountPosts = [];
                        postinfo.ForEach((p) =>
                        {
                            var enclosures = new List<GetItemMyPostListResponse_AccountPostEnclosure>();
                            if (p.AccountPostenclosure != null)
                            {
                                p.AccountPostenclosure.ForEach((e) =>
                                {
                                    var parameters = new List<GetItemMyPostListResponse_AccountPostEnclosureItemParameter>();
                                    if (e.Parameters != null)
                                    {
                                        e.Parameters.ForEach((p) =>
                                        {
                                            parameters.Add(new GetItemMyPostListResponse_AccountPostEnclosureItemParameter()
                                            {
                                                ID = p.ID,
                                                Index = p.Index,
                                                NumberValue = p.NumberValue,
                                                ParamName = p.ParamName,
                                                StringValue = p.StringValue,
                                            });
                                        });
                                    }
                                    enclosures.Add(new GetItemMyPostListResponse_AccountPostEnclosure()
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
                            accountPosts.Add(new GetItemMyPostListResponse_AccountPost
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
                        return new GetItemMyPostListResponse { AccountPosts = accountPosts };
                    }
                    else
                    {
                        return new GetItemMyPostListResponse();
                    }
                }
                catch
                {
                    return new GetItemMyPostListResponse();
                }
            }
            return new GetItemMyPostListResponse();
        }
    }
}
