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
    public class ReceiveListCategoryResponse : ServiceResponseBase
    {
        public Category[] CategoryList;

        public class Category
        {
            public int CategoryID;
            public string CategoryName;
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

            if (this.CategoryList != null && this.CategoryList.Count() > 0)
            {
                for (int i = 0; i < this.CategoryList.Count(); i++)
                {
                    sb.Append("<dict>");

                    sb.Append("     <key>CategoryID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.CategoryList[i].CategoryID.ToString());
                    sb.Append("     <key>CategoryName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.CategoryList[i].CategoryName);
                    sb.Append("     <key>DateCreated</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.CategoryList[i].DateCreated.ToString());

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