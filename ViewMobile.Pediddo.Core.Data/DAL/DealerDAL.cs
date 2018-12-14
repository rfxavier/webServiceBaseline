using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class DealerDAL : DAOBase<Dealer, int>
    {
        public static Dealer Authentication(string pEmail, string password)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dealer>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Dealers.FirstOrDefault(a => a.Employee.Email == pEmail && a.Employee.Password == password);
            }
        }

        public static List<Dealer> ListBySupplierID(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dealer>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Dealers.Where(s => s.Employee.SupplierID == supplierID && s.Active == true).OrderBy(o => o.Employee.FirstName).ToList();
            }
        }

        public static List<Dealer> ListBySupplierID(int supplierID, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dealer>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Dealers.Where(s => s.Employee.SupplierID == supplierID && s.Active == true).OrderBy(o=>o.Employee.FirstName).Skip(skip).Take(10).ToList();
            }
        }

        public static Dealer GetByEmail(string email)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Dealers.FirstOrDefault(s => s.Employee.Email == email && s.Active == true);
            }
        }

        public static Dealer GetBySerialKey(string serialKey)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dealer>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Dealers.FirstOrDefault(a => a.Employee.SerialKey == serialKey);
            }
        }

        public static List<Dealer> ListAdmin(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Dealer>(d => d.Employee);
                db.LoadOptions = dlo;

                return db.Dealers.Where(s => s.Employee.SupplierID == supplierID && s.IsAdmin && s.Active == true).ToList();
            }
        }
    }
}
