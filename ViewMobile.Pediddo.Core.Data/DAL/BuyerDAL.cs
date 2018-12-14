using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class BuyerDAL : DAOBase<Buyer, int>
    {
        public static Buyer Authentication(string Email, string Password)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Buyer>(p => p.Customer);
                db.LoadOptions = dlo;

                return db.Buyers.FirstOrDefault(a => a.Email == Email && a.Password == Password);
            }
        }

        public static Buyer HasEmail(string eMail)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Buyers.FirstOrDefault(a => a.Email == eMail);
            }
        }

        public static Buyer GetBySerialKey(string pSerialKey)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(b => b.Customer);
                db.LoadOptions = dlo;

                return db.Buyers.FirstOrDefault(c => c.SerialKey == pSerialKey && c.Active.Value);
            }
        }

        public static List<Buyer> GetBySellerID(int sellerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.Buyers.Where(b => b.BuyerSellers.Any(bs => bs.SellerID == sellerID)).ToList();
            }
        }

        public static List<Buyer> GetBySellerCustomer(int pSellerID, int pCustomerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.Buyers.Where(b => b.BuyerSellers.Any(bs => bs.SellerID == pSellerID) && 
                                            b.CustomerID == pCustomerID && 
                                            b.Active == true
                                            ).ToList();
            }
        }

        public static List<Buyer> GetByCustomer(int pCustomerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(p => p.Customer);
                db.LoadOptions = dlo;

                return db.Buyers.Where(b => b.CustomerID == pCustomerID && b.Active == true).ToList();
            }
        }

        public static List<Buyer> ListCustomerVip()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(p => p.Customer);
                db.LoadOptions = dlo;

                return db.Buyers.Where(b => b.Customer.Vip == true && b.Active == true).ToList();
            }
        }

        public static List<Buyer> ListCustomerWithCredit()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Buyer>(p => p.Customer);
                db.LoadOptions = dlo;

                return db.Buyers.Where(b => (b.Customer.Credit ?? 0) > 0 && b.Active == true).ToList();
            }
        }
    }
}
