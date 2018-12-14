using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Enumeration;

using Microsoft.WindowsAzure;
using Microsoft.ServiceBus;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class NotificationBLL
    {
        public static Notification GetById(int notificationId)
        {
            return new NotificationDAL().GetById(notificationId);
        }

        public Notification Save(Notification pNotification)
        {
            return new NotificationDAL().Save(pNotification);
        }

        public Notification Update(Notification pNotificationUpdate, Notification pNotificationOrig)
        {
            return new NotificationDAL().Update(pNotificationUpdate, pNotificationOrig);
        }

        public static List<Notification> ListByBuyer(Buyer buyer)
        {
            return NotificationDAL.ListByBuyer(buyer);
        }

        public static int GetCountNotRead(Buyer buyer)
        {
            return NotificationDAL.GetCountNotRead(buyer);
        }

        public static List<Notification> ListByBuyerByPage(Buyer buyer, int skip)
        {
            return NotificationDAL.ListByBuyerByPage(buyer, skip);
        }

        public void SendNotification(Notification notification)
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            //Define the notification hub.
            Microsoft.ServiceBus.Notifications.NotificationHubClient hub =
                Microsoft.ServiceBus.Notifications.NotificationHubClient.CreateClientFromConnectionString(
                    connectionString, "pediddo");

            string[] tag;

            List<Device> _DeviceList = DeviceBLL.ListByBuyerID(notification.BuyerID.Value);
            foreach (Device _Device in _DeviceList)
            {
                // Create a Tag
                tag = new string[] { _Device.PushToken };

                switch (_Device.DeviceOS)
                {
                    case (int)DeviceEnum.DeviceOS.iOS:
                        //Define an iOS alert.
                        var alert = "{\"aps\":{\"alert\":\"" + notification.ContentText + "\"}}";
                        // Send an APNS notification using the template.
                        hub.SendAppleNativeNotificationAsync(alert, tag);
                        break;

                    case (int)DeviceEnum.DeviceOS.Android:
                        // Define a Android jsonPayload.
                        var jsonPayload = "{ \"data\" : {\"title\":\"" + notification.ContentTitle + "\", \"msg\":\"" + notification.ContentText + "\", \"menu\":\"" + notification.Menu.ToString() + "\", \"data\":\"" + notification.Data + "\"}}";
                        // Send an GCM notification using the template.            
                        hub.SendGcmNativeNotificationAsync(jsonPayload, tag);
                        break;

                    case (int)DeviceEnum.DeviceOS.WP:
                        // Define a Windows Phone toast.
                        var mpnsToast =
                            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                            "<wp:Notification xmlns:wp=\"WPNotification\">" +
                                "<wp:Toast>" +
                                    "<wp:Text1>" + notification.ContentText + "</wp:Text1>" +
                                "</wp:Toast> " +
                            "</wp:Notification>";
                        // Send an MPNS notification using the template.            
                        hub.SendMpnsNativeNotificationAsync(mpnsToast, tag);
                        break;

                    default:
                        break;
                }
            }

            this.Save(notification);
        }

        public static List<Notification> ListPendingToSend()
        {
            return NotificationDAL.ListPendingToSend();
        }

        public static List<Notification> ListByListChannel(List<string> listChannel, int pPage)
        {
            return NotificationDAL.ListByListChannel(listChannel, pPage);
        }

        public Notification Update(Notification notificationUpdated)
        {
            NotificationDAL dal = new NotificationDAL();
            Notification notificationOrig = dal.GetById(notificationUpdated.NotificationID);
            Notification notification = new Notification()
            {
                Active = notificationUpdated.Active,
                Activity = notificationUpdated.Activity,
                Argument = notificationUpdated.Argument,
                BuyerID = notificationUpdated.BuyerID,
                Channel = notificationUpdated.Channel,
                ContentText = notificationUpdated.ContentText,
                ContentTitle = notificationUpdated.ContentTitle,
                Data = notificationUpdated.Data,
                DateCreated = notificationUpdated.DateCreated,
                DateToSend = notificationUpdated.DateToSend,
                DeviceOS = notificationUpdated.DeviceOS,
                IsRead = notificationUpdated.IsRead,
                LargeIcon = notificationUpdated.LargeIcon,
                Menu = notificationUpdated.Menu,
                NotificationID = notificationUpdated.NotificationID,
                Send = notificationUpdated.Send,
                SmallIcon = notificationUpdated.SmallIcon,
                Status = notificationUpdated.Status,
                Type = notificationUpdated.Type,
                AppID = notificationUpdated.AppID
            };

            return dal.Update(notification, notificationOrig);
        }

        public void BulkSave(List<Notification> notificationList)
        {
            new NotificationDAL().BulkSave(notificationList);
        }
    }
}
