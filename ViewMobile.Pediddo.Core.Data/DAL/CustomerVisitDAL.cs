using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Enumeration;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CustomerVisitDAL : DAOBase<CustomerVisit, int>
    {
        public static CustomerVisit GetLastByCustomerID(int customerId)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CustomerVisits.Where(v => v.CustomerID == customerId && v.Active == true).OrderByDescending(o => o.DateCreated).FirstOrDefault();
            }
        }

        public static List<CustomerVisit> ListBySeller(int sellerId, int month, int year)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<CustomerVisit>(v => v.Customer);
                dlo.LoadWith<CustomerVisit>(v => v.CustomerVisitProducts);
                dlo.LoadWith<CustomerVisitProduct>(v => v.Product);
                db.LoadOptions = dlo;

                return db.CustomerVisits.Where(v => v.SellerID == sellerId && 
                                                    v.DateCreated.Month == month &&
                                                    v.DateCreated.Year == year &&
                                                    v.Active == true).ToList();
            }
        }
    }
}
