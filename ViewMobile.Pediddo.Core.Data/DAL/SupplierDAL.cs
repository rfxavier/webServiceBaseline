using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SupplierDAL : DAOBase<Supplier, int>
    { 
        public static List<Supplier> ListByBuyer(Buyer pBuyer)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Supplier>(p => p.Employees);
                dlo.LoadWith<Employee>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.BuyerSellers);
                db.LoadOptions = dlo;

                return db.Suppliers.Where(s => s.Employees.Any(sl => sl.Seller.BuyerSellers.Any(bs => bs.BuyerID == pBuyer.BuyerID))).ToList();
            }
        }

        public static List<Supplier> ListByBuyerByPage(Buyer pBuyer, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Supplier>(p => p.Employees);
                dlo.LoadWith<Employee>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.BuyerSellers);
                db.LoadOptions = dlo;

                return db.Suppliers.Where(s => s.Employees.Any(sl => sl.Seller.BuyerSellers.Any(bs => bs.BuyerID == pBuyer.BuyerID))).Skip(pSkip).Take(10).ToList();
            }
        }

        public static List<Supplier> ListByAround()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Suppliers.Where(s => s.SupplierID == 1015).ToList();
            }
        }
        public static Supplier GetBySerialKey(string pSerialKey)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Supplier>(p => p.Employees);
                db.LoadOptions = dlo;

                return db.Suppliers.FirstOrDefault(s => s.SerialKey == pSerialKey);
            }
        }
    }
}
