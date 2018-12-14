using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class PriceListDetailBLL
    {
        public static PriceListDetail GetById(int priceListId, int productId)
        {
            return PriceListDetailDAL.GetById(priceListId, productId);
        }
        

        public void Save(PriceListDetail priceListDetail)
        {
            new PriceListDetailDAL().Save(priceListDetail);
        }


        public PriceListDetail Update(PriceListDetail priceListDetailUpdated)
        {
            PriceListDetailDAL dal = new PriceListDetailDAL();
            PriceListDetail priceListDetailOrig = PriceListDetailDAL.GetById(priceListDetailUpdated.PriceListID, priceListDetailUpdated.ProductID);
            PriceListDetail priceListDetail = new PriceListDetail()
            {
                ProductID = priceListDetailUpdated.ProductID,
                PriceListID = priceListDetailUpdated.PriceListID,
                Margin = priceListDetailUpdated.Margin,
                SalePrice = priceListDetailUpdated.SalePrice,
                MarginDiscount = priceListDetailUpdated.MarginDiscount,
                DiscountPrice = priceListDetailUpdated.DiscountPrice,
                OfferPrice = priceListDetailUpdated.OfferPrice,
                MinimumQuantity = priceListDetailUpdated.MinimumQuantity,
                MaximumDiscount = priceListDetailUpdated.MaximumDiscount,
                Status = priceListDetailUpdated.Status,
                DateCreated = priceListDetailUpdated.DateCreated,
                Active = priceListDetailUpdated.Active
                
            };

            return dal.Update(priceListDetail, priceListDetailOrig);
        }


    }
}
