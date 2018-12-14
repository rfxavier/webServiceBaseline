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
    public class ReceiveMyOrderDetailResponse : ServiceResponseBase
    {
        public int MyOrderID;
        public string Description;
        public decimal Cost;
        public string Observation;
        public Product[] ProductList;

        public class Product
        {
            public int ProductID;
            public string ProductName;
            public int Quantity;
            public decimal Amount;
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

            sb.Append("     <key>MyOrderID</key>");
            sb.AppendFormat("     <string>{0}</string>", this.MyOrderID.ToString());
            sb.Append("     <key>Description</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Description);
            sb.Append("     <key>Cost</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Cost.ToString());
            sb.Append("     <key>Observation</key>");
            sb.AppendFormat("     <string>{0}</string>", this.Observation);

            sb.Append("</dict>");


            if (this.ProductList != null && this.ProductList.Count() > 0)
            {
                for (int i = 0; i < this.ProductList.Count(); i++)
                {

                    sb.Append("<dict>");

                    sb.Append("     <key>ProductID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.ProductList[i].ProductID.ToString());
                    sb.Append("     <key>ProductName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.ProductList[i].ProductName);
                    sb.Append("     <key>Quantity</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.ProductList[i].Quantity.ToString());
                    sb.Append("     <key>Amount</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.ProductList[i].Amount.ToString());

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