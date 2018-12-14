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
    public class ReceiveMyBuyerDetailResponse : ServiceResponseBase
    {
        public int BuyerID;
        public string BuyerName;
        public bool Admin;
        public string PhoneNumber;
        public string Email;
        public int IsBlocked;
        public DateTime DateCreated;

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");


            sb.Append("<dict>");

            if (BuyerID > 0)
            {
                sb.Append("     <key>BuyerID</key>");
                sb.AppendFormat("     <string>{0}</string>", this.BuyerID.ToString());
                sb.Append("     <key>BuyerName</key>");
                sb.AppendFormat("     <string>{0}</string>", this.BuyerName);
                sb.Append("     <key>Admin</key>");
                sb.AppendFormat("     <string>{0}</string>", this.Admin ? "1" : "0");
                sb.Append("     <key>PhoneNumber</key>");
                sb.AppendFormat("     <string>{0}</string>", this.PhoneNumber);
                sb.Append("     <key>Email</key>");
                sb.AppendFormat("     <string>{0}</string>", this.Email);
                sb.Append("     <key>IsBlocked</key>");
                sb.AppendFormat("     <string>{0}</string>", this.IsBlocked.ToString());
                sb.Append("     <key>DateCreated</key>");
                sb.AppendFormat("     <string>{0}</string>", this.DateCreated.ToString());
            }

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