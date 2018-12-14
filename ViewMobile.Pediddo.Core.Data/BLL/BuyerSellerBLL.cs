using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class BuyerSellerBLL
    {
        public BuyerSeller Save(BuyerSeller buyerSeller)
        {
            return new BuyerSellerDAL().Save(buyerSeller);
        }

        public static BuyerSeller GetById(int pBuyerId, int pSellerId)
        {
            return BuyerSellerDAL.GetById(pBuyerId, pSellerId);
        }

        public static List<BuyerSeller> ListByBuyerAndSupplier(int buyerID, int supplierID)
        {
            return BuyerSellerDAL.ListByBuyerAndSupplier(buyerID, supplierID);
        }

        public void BulkDelete(List<BuyerSeller> pBuyerSellerList)
        {
            new BuyerSellerDAL().BulkDelete(pBuyerSellerList);
        }
        public void DeleteBuyerSeller(int buyerID, int sellerID)
        {
            new BuyerSellerDAL().DeleteBuyerSeller(buyerID, sellerID);
        }
    }
}
