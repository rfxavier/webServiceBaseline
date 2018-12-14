using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Data.DAO;
using System.Transactions;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class ProductDAL : DAOBase<Product, int>
    {
        public void BulkSave(List<Product> pProductList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (Product product in pProductList)
                    {
                        db.Products.InsertOnSubmit(product);
                        db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    scope.Complete();
                }
            }
        }

        public static List<Product> ListBySupplierID(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.Category);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.Category.SupplierID == supplierID && p.Active == true).ToList();
            }
        }

        public static List<Product> ListBySupplierIDByPage(int supplierID, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.Category);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.Category.SupplierID == supplierID && p.Active == true)
                    .Skip(skip)
                    .Take(10)
                    .ToList();                
            }
        }


        public static List<Product> ListByCategoryID(int categoryID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.PriceListDetails);
                dlo.LoadWith<PriceListDetail>(p => p.PriceList);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.CategoryID == categoryID && p.Active == true).ToList();
            }
        }

        public static List<Product> ListByCategoryTempID(int categoryID, int customerTypeId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.PriceListDetails);
                dlo.LoadWith<Product>(p => p.Category);
                dlo.LoadWith<PriceListDetail>(p => p.PriceList);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.CategoryID == categoryID && 
                                              p.PriceListDetails.Any(pld=>pld.PriceList.CustomerTypeID == customerTypeId && pld.Active) &&
                                              p.Active == true).ToList();
            }
        }


        public static List<Product> ListByCategoryIDByPage(int categoryID, int skip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.PriceListDetails);
                dlo.LoadWith<PriceListDetail>(p => p.PriceList);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.CategoryID == categoryID && p.Active == true)
                                  .Skip(skip)
                                  .Take(10)
                                  .ToList();
            }
        }

        public static List<Product> ListInOffer(int supplierID, int customerTypeId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Product>(p => p.Category);
                dlo.LoadWith<Product>(p => p.PriceListDetails);
                dlo.LoadWith<PriceListDetail>(p => p.PriceList);
                db.LoadOptions = dlo;

                return db.Products.Where(p => p.Category.SupplierID == supplierID && 
                                              p.PriceListDetails.Any(l=>l.PriceList.CustomerTypeID == customerTypeId && (l.OfferPrice ?? 0) != 0) && 
                                              p.Active.Value).ToList();
            }
        }
    }
}
