using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SupplierRecommendationBLL
    {
        public SupplierRecommendation Save(SupplierRecommendation supplierRecommendation)
        {
            return new SupplierRecommendationDAL().Save(supplierRecommendation);
        }

    }
}