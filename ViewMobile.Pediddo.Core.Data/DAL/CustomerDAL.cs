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
    public class CustomerDAL : DAOBase<Customer, int>
    {
        public static Customer GetByCNPJ(string cnpj)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Customers.FirstOrDefault(c => c.CNPJ == cnpj && c.Active == true);
            }
        }

        public static List<Customer> GetByName(string name)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Bills);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Active == true).ToList();
            }
        }

        public static List<Customer> GetBySellerID(int sellerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Buyers.Any(b => b.BuyerSellers.Any(bs => bs.SellerID == sellerID)) && c.Active == true).ToList();
            }
        }

        public static List<Customer> GetCustomerSync(DateTime LastDateTime)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.CustomerLocations);
                dlo.LoadWith<CustomerLocation>(p => p.City);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.CNPJ != null && c.CNPJ != "" && c.DateCreated >= LastDateTime).ToList();
            }
        }

        public static List<Customer> GetBySellerID(int sellerID, string query, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Customer>(p => p.CustomerLocations);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Buyers.Any(b => b.BuyerSellers.Any(bs => bs.SellerID == sellerID)) && 
                                                (c.Name.Contains(query) || c.BusinessName.Contains(query))
                                                ).ToList();
            }
        }

        public static List<Customer> GetBySellerID(int sellerID, int status, string query, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Buyers.Any(b => b.BuyerSellers.Any(bs => bs.SellerID == sellerID)) &&
                                               c.Status == status &&
                                               (c.Name.Contains(query) || c.Franchise.Contains(query)) &&
                                               c.Active == true)
                                               .OrderByDescending(o=>o.DateCreated)
                                               .Skip(skip)
                                               .Take(10)
                                               .ToList();
            }
        }

        public Customer GetByCustomerID(int customerID)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                return db.Customers.FirstOrDefault(c => c.CustomerID == customerID && c.Active == true);
            }
        }

        public static List<Customer> ListAll()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Active == true).ToList();
            }
        }

        public static List<Customer> ListNameQuery(string query)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<Customer>(p => p.CustomerLocations);
                dlo.LoadWith<CustomerLocation>(p => p.City);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => 
                                          (c.Name + (c.BusinessName ?? "")).Contains(query) &&
                                          c.Status == (int)CustomerEnum.Status.Activo &&
                                          c.Active == true).ToList();
            }
        }

        public static List<Customer> GetBySupplierID(int supplierID, string query, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Customer>(p => p.Buyers);
                dlo.LoadWith<Customer>(p => p.Segment);
                dlo.LoadWith<Customer>(p => p.CustomerLocations);
                dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.Customers.Where(c => c.Buyers.Any(b => b.BuyerSellers.Any(bs => bs.Seller.Employee.SupplierID == supplierID)) &&
                                                (c.Name.Contains(query) || c.BusinessName.Contains(query))
                                                )
                                                .OrderBy(o=>o.Name)
                                                .ToList();
            }
        }

        public static List<Customer> ListForSyncCDS()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Customers.Where(c => c.CNPJ != null && 
                                               c.BusinessName != null &&
                                               c.CNPJ.Trim() != "" && 
                                               c.BusinessName.Trim() != "" && 
                                               c.Active == true)
                                               .ToList();
            }
        }
    }
}
