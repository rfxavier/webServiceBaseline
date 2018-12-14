using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class PromotionReadingBLL
    {
        public PromotionReading Save(PromotionReading promotionReading)
        {
            return new PromotionReadingDAL().Save(promotionReading);
        }
    }
}
