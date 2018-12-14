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
    public class ReceiveListPromotionDetailResponse : ServiceResponseBase
    {

        public int PromotionID;
        public string SupplierName;
        public string SellerName;
        public int Type;
        public string Product;
        public string Description;
        public decimal Cost;
        public string ImageUrl;
        public DateTime ExpirationDate;
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

            sb.Append("     <key>PromotionID</key>");
            sb.AppendFormat("     <string>{0}</string>", this.PromotionID.ToString());
            sb.Append("     <key>SupplierName</key>");
            sb.AppendFormat("     <string>{0}</string>", this.SupplierName);
            sb.Append("     <key>SellerName</key>");
            sb.AppendFormat("     <string>{0}</string>", this.SellerName);
            sb.Append("     <key>Type</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Type.ToString());
            sb.Append("     <key>Producto</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Product);
            sb.Append("     <key>Description</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Description);
            sb.Append("     <key>Cost</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Cost.ToString());
            sb.Append("     <key>ImageUrl</key>");
            sb.AppendFormat("     <string>{0}</string>", this.ImageUrl);
            sb.Append("     <key>ExpirationDate</key>");
            sb.AppendFormat("     <string>{0}</string>", this.ExpirationDate.ToString());
            sb.Append("     <key>DateCreated</key>");
            sb.AppendFormat("     <string>{0}</string>", this.DateCreated.ToString());


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