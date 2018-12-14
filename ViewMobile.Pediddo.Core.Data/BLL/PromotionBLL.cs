using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class PromotionBLL
    {
        public Promotion Save(Promotion promotion)
        {
            return new PromotionDAL().Save(promotion);
        }

        public static Promotion GetById(int promotionID)
        {
            return new PromotionDAL().GetById(promotionID);
        }

        public static Promotion GetFullById(int promotionID)
        {
            return PromotionDAL.GetFullById(promotionID);
        }

        public static List<Promotion> ListByBuyer(Buyer buyer)
        {
            return PromotionDAL.ListByBuyer(buyer);
        }

        public static List<Promotion> ListByBuyerByPage(Buyer buyer, int skip)
        {
            return PromotionDAL.ListByBuyerByPage(buyer, skip);
        }

        public static List<string> ListPromotion(string prefixText, int count, int sellerId)
        {
            
            using (DataClasses1DataContext repositorio = new DataClasses1DataContext())
            {
                var _result = (from lista in repositorio.Promotions
                               where lista.Name.StartsWith(prefixText.Replace("'", "''")) && lista.SellerID == sellerId
                               select new { Name = lista.Name }).Take(count);

                List<string> t = new List<string>();

                foreach (var tt in _result)
                {
                    t.Add(tt.Name.ToUpperInvariant());
                }
                return t;
            }
        }

        public static List<Promotion> ListBySeller(Seller seller)
        {
            return PromotionDAL.ListBySeller(seller);
        }

        public Promotion Update(Promotion pPromotionUpdate, Promotion pPromotionOrig)
        {
            return new PromotionDAL().Update(pPromotionUpdate, pPromotionOrig);
        }

        public static List<Promotion> ListBySellerAndName(Seller seller, string name)
        {
            return PromotionDAL.ListBySellerAndName(seller, name);
        }

        public static List<Promotion> ListBySellerAndProduct(Seller seller, string product)
        {
            return PromotionDAL.ListBySellerAndProduct(seller, product);
        }
    }
}
