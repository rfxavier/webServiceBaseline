using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Transactions;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class BuyerPromotionDAL : DAOBase<BuyerPromotion, int>
    {
        public static List<BuyerPromotion> ListNotRead(Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                dlo.LoadWith<BuyerPromotion>(p => p.Promotion);
                db.LoadOptions = dlo;

                return db.BuyerPromotions.Where(b => b.BuyerID == pBuyer.BuyerID && b.PromotionReadings.Count() == 0 && b.Promotion.Active == true).ToList();
            }
        }

        public void BulkSave(List<BuyerPromotion> pBuyerPromotionList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (BuyerPromotion buyerPromotion in pBuyerPromotionList)
                    {
                        db.BuyerPromotions.InsertOnSubmit(buyerPromotion);
                        db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    scope.Complete();
                }
            }
        }

        public static List<BuyerPromotion> ListByPromotion(Promotion promotion)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.BuyerPromotions.Where(b => b.PromotionID == promotion.PromotionID).ToList();
            }
        }

        public static List<BuyerPromotion> ListFullByPromotion(Promotion promotion)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerPromotion>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                db.LoadOptions = dlo;

                return db.BuyerPromotions.Where(b => b.PromotionID == promotion.PromotionID).ToList();
            }
        }

        public static List<BuyerPromotion> ListFullByPromotionBuyerName(Promotion promotion, string buyerName)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerPromotion>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                db.LoadOptions = dlo;

                return db.BuyerPromotions.Where(b => b.PromotionID == promotion.PromotionID &&
                                                ((b.Buyer.FirstName ?? "").ToUpper() + " " + (b.Buyer.LastName ?? "").ToUpper()).Contains(buyerName.ToUpper())
                                                ).ToList();
            }
        }

        public static List<BuyerPromotion> ListFullByPromotionCustomerName(Promotion promotion, string customerName)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerPromotion>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                db.LoadOptions = dlo;

                return db.BuyerPromotions.Where(b => b.PromotionID == promotion.PromotionID &&
                                               b.Buyer.Customer.Name.ToUpper().Contains(customerName.ToUpper())
                                               ).ToList();
            }
        }

        public static List<BuyerPromotion> ListFullByPromotionCustomerCNPJ(Promotion promotion, string cnpj)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerPromotion>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<BuyerPromotion>(p => p.PromotionReadings);
                db.LoadOptions = dlo;

                return db.BuyerPromotions.Where(b => b.PromotionID == promotion.PromotionID &&
                    b.Buyer.Customer.CNPJ == cnpj
                    ).ToList();
            }
        }
    }
}
