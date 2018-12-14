using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SellerTrackPointBLL
    {
        public SellerTrackPoint Save(SellerTrackPoint sellerTrackPoint)
        {
            return new SellerTrackPointDAL().Save(sellerTrackPoint);
        }

        public static List<spListTodayTravelPointResult> ListTodayTravelPoint(int sellerId, DateTime fecha)
        {
            return SellerTrackPointDAL.ListTodayTravelPoint(sellerId, fecha);
        }
    }
}
