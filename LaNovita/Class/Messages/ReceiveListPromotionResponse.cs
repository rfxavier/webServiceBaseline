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
    public class ReceiveListPromotionResponse : ServiceResponseBase
    {
        public Promotion[] PromotionList;

        public class Promotion
        {
            public int PromotionID;
            public string SupplierName;
            public int Type;
            public string Product;
            public DateTime DateCreated;
        }

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");

            if (this.PromotionList != null && this.PromotionList.Count() > 0)
            {
                for (int i = 0; i < this.PromotionList.Count(); i++)
                {
                    sb.Append("<dict>");

                    sb.Append("     <key>PromotionID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.PromotionList[i].PromotionID.ToString());
                    sb.Append("     <key>SupplierName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.PromotionList[i].SupplierName);
                    sb.Append("     <key>Type</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.PromotionList[i].Type.ToString());
                    sb.Append("     <key>Producto</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.PromotionList[i].Product);
                    sb.Append("     <key>DateCreated</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.PromotionList[i].DateCreated.ToString());

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