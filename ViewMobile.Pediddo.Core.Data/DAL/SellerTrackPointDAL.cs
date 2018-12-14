using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SellerTrackPointDAL : DAOBase<SellerTrackPoint, int>
    {
        public static List<spListTodayTravelPointResult> ListTodayTravelPoint(int sellerId, DateTime fecha)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.spListTodayTravelPoint(sellerId, fecha).ToList();
            }
        }
    }
}
