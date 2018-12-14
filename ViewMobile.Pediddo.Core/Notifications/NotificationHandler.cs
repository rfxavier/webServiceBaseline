using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Notifications
{
    public class NotificationHandler
    {
        public static string GetMessage(string cultureString, string messageCode)
        {
            string returnMessage = "";
            switch (cultureString)
            {
                //case "es":
                //    returnMessage = NotificationMessageEs_PY.ResourceManager.GetString(messageCode);
                //    break;
                //case "pt":
                //    returnMessage = NotificationMessagePt_BR.ResourceManager.GetString(messageCode);
                //    break;
                //default:
                //    returnMessage = NotificationMessagePt_BR.ResourceManager.GetString(messageCode);
                //    break;

            };

            return returnMessage;
        }

        public static string GetEmailHtml(string cultureString, string messageCode)
        {
            string returnMessage = "";
            switch (cultureString)
            {
                case "es":
                    returnMessage = EmailHtmlMessageEs_PY.ResourceManager.GetString(messageCode);
                    break;
                case "pt":
                    returnMessage = EmailHtmlMessageEs_PY.ResourceManager.GetString(messageCode);
                    break;
                default:
                    returnMessage = EmailHtmlMessageEs_PY.ResourceManager.GetString(messageCode);
                    break;

            };

            return returnMessage;
        }
    }
}
