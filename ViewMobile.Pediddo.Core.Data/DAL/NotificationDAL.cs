using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class NotificationDAL : DAOBase<Notification, int>
    {
        public static List<Notification> ListByBuyer(Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Notifications.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .ToList();
            }
        }

        public static List<Notification> ListByBuyerByPage(Buyer pBuyer, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Notifications.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .Skip(pSkip)
                                  .Take(10)
                                  .ToList();
            }
        }

        public static int GetCountNotRead(Buyer buyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Notifications.Where(c => c.BuyerID == buyer.BuyerID &&
                                              c.IsRead == false &&
                                              c.Active == true)
                                  .ToList().Count();
            }
        }

        public static List<Notification> ListPendingToSend()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Notifications.Where(p => p.DateToSend <= DateTime.Now && p.Send == false && p.Active == true).ToList();
            }
        }

        public static List<Notification> ListByListChannel(List<string> channelList, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                if (page == 0)
                {
                    return db.Notifications.Where(p => channelList.Contains(p.Channel) && p.Send == true && p.Active == true).OrderByDescending(o => o.DateToSend).ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.Notifications.Where(p => channelList.Contains(p.Channel) && p.Send == true && p.Active == true).OrderByDescending(o => o.DateToSend).Skip(skip).Take(10).ToList();
                }
            }
        }

        public void BulkSave(List<Notification> notificationList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Notification notification in notificationList)
                    {
                        db.Notifications.InsertOnSubmit(notification);
                        db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    scope.Complete();
                }
            }
        }
    }
}
