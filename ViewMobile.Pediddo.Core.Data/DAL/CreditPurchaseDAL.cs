using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CreditPurchaseDAL : DAOBase<CreditPurchase, int>
    {
        public static List<CreditPurchase> ListByStatus(int status)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<CreditPurchase>(cp => cp.Customer);
                dlo.LoadWith<Customer>(cp => cp.Buyers);
                db.LoadOptions = dlo;

                return db.CreditPurchases.Where(cp => cp.Status == status).ToList();
            }
        }
    }
}
