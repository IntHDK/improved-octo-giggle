using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebReactApp.Server.ModelObjects;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.ItemService
{
    public partial class ItemManager
    {
        //사용자 우편 리스트
        public List<AccountPost> GetNotExpiredAccountPostsByAccountID(Guid AccountID)
        {
            var curacc = appDbContext.Accounts.Include(a => a.AccountPosts).ThenInclude(p => p.AccountPostenclosure).Where(a => a.ID == AccountID).FirstOrDefault();
            if (curacc != default)
            {
                return curacc.AccountPosts.Where(i => i.ExpireAt >= DateTime.Now).ToList();
            }
            else
            {
                return [];
            }
        }
        //우편 내용 획득
        public AccountPost? GetPostByPostID(Guid PostID)
        {
            var curent = appDbContext.AccountPosts.Where(a => a.ID == PostID).FirstOrDefault();
            if (curent != default)
            {
                return curent;
            }
            else
            {
                return null;
            }
        }
        //우편 열람 체크
        public bool MarkAsReadPostByPostID(Guid PostID)
        {
            var curent = appDbContext.AccountPosts.Where(a => a.ID == PostID).FirstOrDefault();
            if (curent != default)
            {
                curent.IsRead = true;
                appDbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        //우편 동봉 아이템 획득
        public bool TakeEnclosureOnPost(Guid PostID)
        {
            var curent = appDbContext.AccountPosts.Include(p => p.AccountPostenclosure).ThenInclude(e => e.Parameters).Where(a => a.ID == PostID).FirstOrDefault();
            if (curent != default)
            {
                if (curent.IsEnclosureTaken)
                {
                    return false;
                }
                curent.IsRead = true;
                curent.IsEnclosureTaken = true;
                List<AccountItem> targetitems = new List<AccountItem>();
                if (curent.AccountPostenclosure == null)
                {
                    //아이템 칸 비어있음
                    appDbContext.SaveChanges();
                    return true;
                }
                foreach(var enclosure in curent.AccountPostenclosure)
                {
                    var parameters = new List<AccountItemParameters>();
                    foreach(var param in enclosure.Parameters)
                    {
                        parameters.Add(new AccountItemParameters()
                        {
                            Index = param.Index,
                            NumberValue = param.NumberValue,
                            ParamName = param.ParamName,
                            StringValue = param.StringValue,
                        });
                    }
                    targetitems.Add(new AccountItem()
                    {
                        ExpireAt = enclosure.ExpireAt,
                        ItemMetaName = enclosure.ItemMetaName,
                        Quantity = enclosure.Quantity,
                        Type = enclosure.Type,
                        Parameters = parameters
                    });
                }
                if (AddAccountItems(curent.AccountID, targetitems))
                {
                    appDbContext.SaveChanges();
                    return true;
                }
                else
                {
                    curent.IsEnclosureTaken = false;
                    appDbContext.SaveChanges();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //확인한 우편 및 아이템 획득한 우편 제거
        public bool CleanUpPostWhereIsReadAndIsTakenByAccountID(Guid AccountID)
        {
            var removals = appDbContext.AccountPosts.Where(p =>
            p.ExpireAt < DateTime.Now &&
            p.IsRead && p.IsEnclosureTaken && p.AccountID == AccountID).ToArray();

            //TODO: 벌크 쓸 수 있는지 확인
            appDbContext.AccountPosts.RemoveRange(removals);
            appDbContext.SaveChanges();
            return true;
        }

        //conditionexpression에 해당하는 Account에 대한 우편 생성
        public bool AddPostByAccountCondition(AccountPost PostTemplate, Expression<Func<Account, bool>> conditionexpression)
        {
            List<AccountPostEnclosure> enclosures = new List<AccountPostEnclosure>();
            var targetaccs = appDbContext.Accounts.Where(conditionexpression).ToList();
            List<AccountPost> bulks = new List<AccountPost>();
            PostTemplate.AccountPostenclosure.ForEach((e) =>
            {
                var newenc = new AccountPostEnclosure()
                {
                    CreatedTime = e.CreatedTime,
                    ExpireAt = e.ExpireAt,
                    ItemMetaName = e.ItemMetaName,
                    Quantity = e.Quantity,
                    Type = e.Type,
                };
                var itp = new List<AccountPostEnclosureItemParameter>();
                e.Parameters.ForEach((p) =>
                {
                    var paraminfo = new AccountPostEnclosureItemParameter()
                    {
                        AccountPostEnclosure = newenc,
                        AccountPostEnclosureID = newenc.ID,
                        Index = p.Index,
                        NumberValue = p.NumberValue,
                        ParamName = p.ParamName,
                        StringValue = p.StringValue,
                    };
                    itp.Add(paraminfo);
                });
                newenc.Parameters = itp;
                enclosures.Add(newenc);
            });
            foreach (var acc in targetaccs)
            {
                //TODO: 벌크 작업시 related object들도 다 끌어서 쓸 수 있는지 확인
                //bulks.Add();
                appDbContext.AccountPosts.Add(new AccountPost
                {
                    ID = Guid.NewGuid(), //Bulkinsert에서는 Add시 ID 생성규칙이 적용되지 않는듯?
                    Account = acc,
                    AccountID = acc.ID,
                    Context = PostTemplate.Context,
                    CreatedTime = DateTime.Now,
                    AccountPostenclosure = enclosures,
                    ExpireAt = PostTemplate.ExpireAt,
                    IsEnclosureTaken = false,
                    IsRead = false
                });
            }
            appDbContext.SaveChanges();
            return true;
        }
    }
}
