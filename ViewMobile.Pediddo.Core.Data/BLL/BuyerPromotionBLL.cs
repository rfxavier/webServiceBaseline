using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class BuyerPromotionBLL
    {
        public BuyerPromotion Save(BuyerPromotion buyerPromotion)
        {
            return new BuyerPromotionDAL().Save(buyerPromotion);
        }

        public void BulkSave(List<BuyerPromotion> buyerPromotionList)
        {
            new BuyerPromotionDAL().BulkSave(buyerPromotionList);
        }

        public static List<BuyerPromotion> ListNotRead(Buyer buyer)
        {
            return BuyerPromotionDAL.ListNotRead(buyer);
        }

        public static List<BuyerPromotion> ListByPromotion(Promotion promotion)
        {
            return BuyerPromotionDAL.ListByPromotion(promotion);
        }

        public static List<BuyerPromotion> ListFullByPromotion(Promotion pPromotion)
        {
            return BuyerPromotionDAL.ListFullByPromotion(pPromotion);
        }

        public static List<BuyerPromotion> ListFullByPromotionBuyerName(Promotion promotion, string buyerName)
        {
            return BuyerPromotionDAL.ListFullByPromotionBuyerName(promotion, buyerName);
        }

        public static List<BuyerPromotion> ListFullByPromotionCustomerName(Promotion promotion, string supplierName)
        {
            return BuyerPromotionDAL.ListFullByPromotionCustomerName(promotion, supplierName);
        }

        public static List<BuyerPromotion> ListFullByPromotionCustomerCNPJ(Promotion promotion, string cnpj)
        {
            return BuyerPromotionDAL.ListFullByPromotionCustomerCNPJ(promotion, cnpj);
        }
    }
}
