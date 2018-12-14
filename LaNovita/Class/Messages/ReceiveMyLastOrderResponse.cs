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
    public class ReceiveMyLastOrderResponse : ServiceResponseBase
    {
        public int MyOrderID;
        public string Description;
        public string InvoiceNumber;
        public decimal? Cost;
        public string DateCreated;
        public string ReceiptCode;
        public int SupplierID;
        public string SupplierName;
        public int SellerID;

        public Product[] ProductList;

        public class Product
        {
            public int ProductId;
            public string ProductName;
            public int Quantity;
            public decimal Amount;
            public int ImageId;
            public string PriceStr;
            public string UnitMeasure;
        }

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");

            sb.Append("<dict>");

            if (this.MyOrderID > 0)
            {
                sb.Append("     <key>MyOrderID</key>");
                sb.AppendFormat("     <string>{0}</string>", this.MyOrderID.ToString());
                sb.Append("     <key>Product</key>");
                sb.AppendFormat("     <string>{0}</string>", this.Description);
                sb.Append("     <key>InvoiceNumber</key>");
                sb.AppendFormat("     <string>{0}</string>", this.InvoiceNumber);
                sb.Append("     <key>Cost</key>");
                sb.AppendFormat("     <string>{0}</string>", this.Cost == null ? "" : this.Cost.ToString());
                sb.Append("     <key>DateCreated</key>");
                sb.AppendFormat("     <string>{0}</string>", this.DateCreated.ToString());
                sb.Append("     <key>ReceiptCode</key>");
                sb.AppendFormat("     <string>{0}</string>", this.ReceiptCode);
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