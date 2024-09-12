using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.ItemService
{
    public partial class ItemManager
    {
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
