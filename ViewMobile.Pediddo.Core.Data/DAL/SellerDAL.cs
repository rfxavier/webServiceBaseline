using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SellerDAL : DAOBase<Seller, int>
    {
        public static Seller Authentication(string pEmail, string password)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Seller>(d => d.Employee);
                dlo.LoadWith<Seller>(d => d.UserRoles);
                dlo.LoadWith<UserRole>(d => d.Role);
                db.LoadOptions = dlo;

                return db.Sellers.FirstOrDefault(a => a.Employee.Email == pEmail && a.Employee.Password == password);
            }
        }

        public static List<Seller> ListBySupplierID(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Seller>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Sellers.Where(s => s.Employee.SupplierID == supplierID && s.Active == true).ToList();
            }
        }

        public static Seller GetByEmail(string email)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Sellers.FirstOrDefault(s => s.Employee.Email == email && s.Active == true);
            }
        }

        public static Seller GetBySerialKey(string serialKey)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Seller>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Sellers.FirstOrDefault(a => a.Employee.SerialKey == serialKey);
            }
        }

        public static List<spLastSellerLocationResult> ListLastSellerLocation(DateTime fecha)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.CommandTimeout = 500;
                return db.spLastSellerLocation(fecha.Date).ToList();
            }
        }

        public static List<spLastSellerAliveResult> ListLastSellerAlive(DateTime fecha)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.CommandTimeout = 500;
                return db.spLastSellerAlive(fecha).ToList();
            }
        }

        public static List<Seller> ListAdmin(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Seller>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Sellers.Where(s => s.Employee.SupplierID == supplierID && s.IsAdmin && s.Active == true).ToList();
            }
        }

        public static List<Seller> ListAll(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Seller>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Sellers.Where(s => s.Employee.SupplierID == supplierID && s.Active == true).ToList();
            }
        }
    }
}
