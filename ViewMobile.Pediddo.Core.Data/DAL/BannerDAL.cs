using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class BannerDAL : DAOBase<Banner, int>
    {
        public static List<Banner> ListByType(int bannerType)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Banners.Where(b => b.Type == bannerType && b.BannerActive && b.Active).ToList();
            }
        }
    }
}
