using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SellerWorkingDayDAL : DAOBase<SellerWorkingDay, int>
    {
        public static SellerWorkingDay GetByDateAndType(int sellerId, DateTime date, int type)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.SellerWorkingDays.FirstOrDefault(w => w.SellerID == sellerId && 
                                                                w.WorkingDayDate.Date == date && 
                                                                w.Type == type);
            }
        }

        public static SellerWorkingDay GetLast(int sellerId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.SellerWorkingDays.Where(w => w.SellerID == sellerId &&
                                                       w.Active == true)
                                                       .OrderByDescending(o=>o.WorkingDayDate)
                                                       .FirstOrDefault();
            }
        }
    }
}
