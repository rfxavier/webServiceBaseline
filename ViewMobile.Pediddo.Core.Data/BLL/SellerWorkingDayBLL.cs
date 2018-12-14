using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SellerWorkingDayBLL
    {
        public SellerWorkingDay Save(SellerWorkingDay sellerWorkingDay)
        {
            return new SellerWorkingDayDAL().Save(sellerWorkingDay);
        }

        public static SellerWorkingDay GetByDateAndType(int sellerId, DateTime date, int type)
        {
            return SellerWorkingDayDAL.GetByDateAndType(sellerId, date, type);
        }

        public static SellerWorkingDay GetLast(int sellerId)
        {
            return SellerWorkingDayDAL.GetLast(sellerId);
        }
    }
}
