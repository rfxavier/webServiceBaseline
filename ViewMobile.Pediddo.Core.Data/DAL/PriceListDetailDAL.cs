using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class PriceListDetailDAL : DAOBase<PriceListDetail, int>
    {
        public static PriceListDetail GetById(int priceListId, int productId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.PriceListDetails.FirstOrDefault(pld => pld.PriceListID == priceListId && pld.ProductID == productId && pld.Active);
            }
        }
    }
}
