using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class PromotionDAL : DAOBase<Promotion, int>
    {
        public static Promotion GetFullById(int pPromotionID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Promotion>(p => p.BuyerPromotions);
                dlo.LoadWith<Promotion>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<Employee>(p => p.Supplier);
                db.LoadOptions = dlo;

                return db.Promotions.FirstOrDefault(p => p.PromotionID == pPromotionID );
            }
        }

        public static List<Promotion> ListByBuyer(Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Promotion>(p => p.BuyerPromotions);
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                dlo.LoadWith<Promotion>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<Employee>(p => p.Supplier);
                db.LoadOptions = dlo;

                return db.Promotions.Where(c => c.BuyerPromotions.Any(a => a.BuyerID == pBuyer.BuyerID && 
                                                                      (a.PromotionReadings.Count() == 0 || a.PromotionReadings.OrderByDescending(o=>o.PromotionReadingID).FirstOrDefault().Status != (int)PromotionReadingEnum.Status.Aceptado)) &&
                                              DateTime.Now <= c.ExpirationDate &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .ToList();
            }
        }

        public static List<Promotion> ListByBuyerByPage(Buyer pBuyer, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Promotion>(p => p.BuyerPromotions);
                dlo.LoadWith<Promotion>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<Employee>(p => p.Supplier);
                db.LoadOptions = dlo;

                return db.Promotions.Where(c => c.BuyerPromotions.Any(a => a.BuyerID == pBuyer.BuyerID &&
                                                                      (a.PromotionReadings.Count() == 0 || a.PromotionReadings.OrderByDescending(o => o.PromotionReadingID).FirstOrDefault().Status != (int)PromotionReadingEnum.Status.Aceptado)) &&
                                              DateTime.Now <= c.ExpirationDate &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .Skip(pSkip)
                                  .Take(10)
                                  .ToList();
            }
        }

        public static List<Promotion> ListBySeller(Seller seller)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Promotions.Where(p => p.SellerID == seller.SellerID && p.ExpirationDate >= DateTime.Now && p.Active == true).ToList();
            }
        }

        public static List<Promotion> ListBySellerAndName(Seller pSeller, string pName)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return (from p in db.Promotions
                               where p.Name.ToUpper().Contains(pName.ToUpper()) && p.SellerID == pSeller.SellerID
                               select p).ToList();
            }
        }

        public static List<Promotion> ListBySellerAndProduct(Seller pSeller, string pProduct)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return (from p in db.Promotions
                        where p.Product.ToUpper().Contains(pProduct.ToUpper()) && p.SellerID == pSeller.SellerID
                        select p).ToList();
            }
        }
    }
}
