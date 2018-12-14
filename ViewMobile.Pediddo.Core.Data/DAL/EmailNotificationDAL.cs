using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class EmailNotificationDAL : DAOBase<EmailNotification, int>
    {
        public static List<EmailNotification> ListPendingToSend()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.EmailNotifications.Where(p => p.DateToSend <= DateTime.Now && p.Send == false && p.Active == true).ToList();
            }
        }
    }
}
