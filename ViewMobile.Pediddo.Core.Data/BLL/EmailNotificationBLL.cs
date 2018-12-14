using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class EmailNotificationBLL
    {
        public EmailNotification Save(EmailNotification emailNotification)
        {
            return new EmailNotificationDAL().Save(emailNotification);
        }

        public EmailNotification Update(EmailNotification emailNotificationUpd, EmailNotification emailNotification)
        {
            return new EmailNotificationDAL().Update(emailNotificationUpd, emailNotification);
        }

        public static List<EmailNotification> ListPendingToSend()
        {
            return EmailNotificationDAL.ListPendingToSend();
        }
    }
}
