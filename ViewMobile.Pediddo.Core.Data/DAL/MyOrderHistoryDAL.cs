using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class MyOrderHistoryDAL : DAOBase<MyOrderHistory, int>
    {
        public static List<MyOrderHistory> GetByMyOrderID(int myOrderID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.MyOrderHistories.Where(op => op.MyOrderID == myOrderID && op.Active == true).ToList();
            }
        }

        public static MyOrderHistory GetLast(int myOrderID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrderHistory>(h => h.Seller);
                dlo.LoadWith<Seller>(h => h.Employee);
                db.LoadOptions = dlo;

                return db.MyOrderHistories.Where(h => h.MyOrderID == myOrderID && h.Active == true).OrderByDescending(o => o.DateCreated).FirstOrDefault();
            }
        }
    }
}
