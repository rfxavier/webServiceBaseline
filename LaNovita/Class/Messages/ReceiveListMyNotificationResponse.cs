using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListMyNotificationResponse : ServiceResponseBase
    {
        public MyNotification[] MyNotificationList;

        public class MyNotification
        {
            public int NotificationID;
            public int Type;
            public string Title;
            public string Message;
            public int Status;
            public bool IsRead;
            public int Menu;
            public string Data;
            public string DateCreated;
        }

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");

            if (this.MyNotificationList != null && this.MyNotificationList.Count() > 0)
            {
                for (int i = 0; i < this.MyNotificationList.Count(); i++)
                {
                    sb.Append("<dict>");

                    sb.Append("     <key>NotificationID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].NotificationID.ToString());
                    sb.Append("     <key>Type</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Type.ToString());
                    sb.Append("     <key>Title</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Title);
                    sb.Append("     <key>Message</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Message);
                    sb.Append("     <key>Status</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Status.ToString());
                    sb.Append("     <key>IsRead</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].IsRead == true ? "1" : "0");
                    sb.Append("     <key>Menu</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Menu.ToString());
                    sb.Append("     <key>Data</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].Data);
                    sb.Append("     <key>DateCreated</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyNotificationList[i].DateCreated.ToString());

                    sb.Append("</dict>");
                }
            }

            sb.Append("<dict>");

            ///status
            sb.Append("     <key>Status</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Status.ToString());
            sb.Append("     <key>ErrorCode</key>");
            sb.AppendFormat("     <string>{0}</string>", this.ErrorCode);
            sb.Append("     <key>ErrorMessage</key>");
            sb.AppendFormat("     <string>{0}</string>", this.ErrorMessage);


            sb.Append("</dict>");


            sb.Append("</array>");
            sb.Append("</plist>");

            Playlist = sb.ToString();

            return sb.ToString();
        }

        public string ToErrorString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Customer Response:" + Environment.NewLine);
            sb.Append("============================= " + Environment.NewLine);
            sb.Append("Status              = " + this.Status.ToString() + Environment.NewLine);
            sb.Append("ErrorCode           = " + this.ErrorCode + Environment.NewLine);
            sb.Append("ErrorMessage        = " + this.ErrorMessage + Environment.NewLine);
            return sb.ToString();
        }
        #endregion
    }
}