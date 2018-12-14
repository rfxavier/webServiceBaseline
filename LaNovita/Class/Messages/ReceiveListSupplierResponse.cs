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
    public class ReceiveListSupplierResponse : ServiceResponseBase
    {
        public Supplier[] SupplierList;
        public class Supplier
        {
            public int SupplierID;
            public string Name;
            public string CNPJ;
            public string PhoneNumber;
            public Seller[] SellerList;
        }

        public class Seller
        {
            public int SellerID;
            public string Name;
            public int SupplierID;
        }

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");

            

            if (this.SupplierList != null && this.SupplierList.Count() > 0)
            {
                for (int i = 0; i < this.SupplierList.Count(); i++)
                {
                    sb.Append("<dict>");

                    sb.Append("     <key>SupplierID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].SupplierID.ToString());
                    sb.Append("     <key>Name</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].Name);
                    sb.Append("     <key>CNPJ</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].CNPJ);
                    sb.Append("     <key>PhoneNumber</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].PhoneNumber);

                    sb.Append("     <key>SellerID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].SellerList[0].SellerID.ToString());
                    sb.Append("     <key>SellerName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].SellerList[0].Name);

                    //sb.Append("<array>");
                    //for (int j = 0; j < this.SupplierList[i].SellerList.Count(); j++ )
                    //{
                    //    sb.Append("<dict>");

                    //    sb.Append("     <key>SellerID</key>");
                    //    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].SellerList[j].SellerID.ToString());
                    //    sb.Append("     <key>Name</key>");
                    //    sb.AppendFormat("     <string>{0}</string>", this.SupplierList[i].SellerList[j].Name);

                    //    sb.Append("</dict>");
                    //}
                    //sb.Append("</array>");

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