using Microsoft.EntityFrameworkCore;
using WebReactApp.Server.Data;
using WebReactApp.Server.ModelObjects;
using WebReactApp.Server.ModelObjects.Identity;

namespace WebReactApp.Server.Services.ItemService
{
    public partial class ItemManager
    {
        private readonly AppDbContext appDbContext;

        public ItemManager(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public List<AccountItem> GetNotExpiredAccountItemsByAccountID(Guid AccountID)
        {
            var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
            if (curacc != default)
            {
                return curacc.AccountItems.Where(i => i.ExpireAt >= DateTime.Now).ToList();
            }
            else
            {
                return [];
            }
        }

        //사용자 아이템 추가 (단일)
        public bool AddAccountItem(Guid AccountID, AccountItem accountItem)
        {
            var tx = appDbContext.Database.BeginTransaction();
            try
            {
                var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
                if (curacc != default)
                {
                    switch (accountItem.Type)
                    {
                        case AccountItemType.Currency_Cash:
                            curacc.Currency_Cash += accountItem.Quantity;
                            appDbContext.SaveChanges(); tx.Commit();
                            break;
                        case AccountItemType.Currency_Point:
                            curacc.Currency_Point += accountItem.Quantity;
                            appDbContext.SaveChanges(); tx.Commit();
                            break;
                        case AccountItemType.Item:
                        case AccountItemType.Item_Package:
                            if (accountItem.Parameters.Count <= 0)
                            {
                                //파라메터 없는 아이템에 한해서 Quantity 겹치기
                                var accent = curacc.AccountItems.Where(i => i.ItemMetaName == accountItem.ItemMetaName && i.ExpireAt == accountItem.ExpireAt).FirstOrDefault();
                                if (accent != null)
                                {
                                    if (accent.Parameters.Count <= 0)
                                    {
                                        accent.Quantity += accountItem.Quantity; // 메타명과 만료기간이 같으면 Quantity를 증가
                                        break;
                                    }
                                }
                            }
                            var newitem = new AccountItem()
                            {
                                Account = curacc,
                                AccountID = curacc.ID,
                                ExpireAt = accountItem.ExpireAt,
                                CreatedTime = DateTime.Now,
                                ItemMetaName = accountItem.ItemMetaName,
                                Parameters = accountItem.Parameters.ToList(),
                                Quantity = accountItem.Quantity,
                                Type = accountItem.Type,
                            };
                            curacc.AccountItems.Add(accountItem);
                            appDbContext.SaveChanges(); tx.Commit();
                            break;
                        default:
                            break;
                    }
                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return false;
            }
            return true;
        }

        //사용자 아이템 추가(여러개)
        public bool AddAccountItems(Guid AccountID, List<AccountItem> accountItems)
        {
            var tx = appDbContext.Database.BeginTransaction();
            try
            {
                var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
                if (curacc != default)
                {
                    foreach(var accountItem in accountItems)
                    {
                        switch (accountItem.Type)
                        {
                            case AccountItemType.Currency_Cash:
                                curacc.Currency_Cash += accountItem.Quantity;
                                break;
                            case AccountItemType.Currency_Point:
                                curacc.Currency_Point += accountItem.Quantity;
                                break;
                            case AccountItemType.Item:
                            case AccountItemType.Item_Package:
                                if (accountItem.Parameters.Count <= 0)
                                {
                                    //파라메터 없는 아이템에 한해서 Quantity 겹치기
                                    var accent = curacc.AccountItems.Where(i => i.ItemMetaName == accountItem.ItemMetaName && i.ExpireAt == accountItem.ExpireAt).FirstOrDefault();
                                    if (accent != null)
                                    {
                                        accent.Quantity += accountItem.Quantity; // 메타명과 만료기간이 같으면 Quantity를 증가
                                        break;
                                    }
                                }
                                
                                var newitem = new AccountItem()
                                {
                                    Account = curacc,
                                    AccountID = curacc.ID,
                                    ExpireAt = accountItem.ExpireAt,
                                    CreatedTime = DateTime.Now,
                                    ItemMetaName = accountItem.ItemMetaName,
                                    Parameters = accountItem.Parameters.ToList(),
                                    Quantity = accountItem.Quantity,
                                    Type = accountItem.Type,
                                };
                                curacc.AccountItems.Add(accountItem);
                                break;
                            default:
                                break;
                        }
                    }

                    appDbContext.SaveChanges(); tx.Commit();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return false;
            }
            return true;
        }
        public bool CountUpAccountItemQuantity(Guid AccountID, Guid AccountItemID, int QuantityDelta)
        {
            var tx = appDbContext.Database.BeginTransaction();
            try
            {
                var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
                if (curacc != default)
                {
                    var curitem = curacc.AccountItems.Where(i => i.ID == AccountItemID).FirstOrDefault();
                    if (curitem != default)
                    {
                        curitem.Quantity = curitem.Quantity + QuantityDelta;
                        if (curitem.Quantity <= 0)
                        {
                            curacc.AccountItems.Remove(curitem);
                        }
                        appDbContext.SaveChanges(); tx.Commit();

                        return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateExpirationAccountItem(Guid AccountID, Guid AccountItemID, DateTime NewExpireAt)
        {
            var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
            if (curacc != default)
            {
                var curitem = curacc.AccountItems.Where(i => i.ID == AccountItemID).FirstOrDefault();
                if (curitem != default)
                {
                    curitem.ExpireAt = NewExpireAt;
                    appDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveAccountItem(Guid AccountID, Guid AccountItemID)
        {
            var curacc = appDbContext.Accounts.Include(a => a.AccountItems).Where(a => a.ID == AccountID).FirstOrDefault();
            if (curacc != default)
            {
                var curitem = curacc.AccountItems.Where(i => i.ID == AccountItemID).FirstOrDefault();
                if (curitem != default)
                {
                    curacc.AccountItems.Remove(curitem);
                    appDbContext.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
