using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveBuyerNotificationListResponse : ServiceResponseBase
    {
        public List<Notification> NotificationList;

        public class Notification
        {
            public int NotificationID;
            public int Activity;
            public string Argument;
            public string Title;
            public string Message;
            public bool IsRead;
            public string DateToSend;
            public string DateCreated;
        }
    }
}