using System;
using System.Linq;
using System.Text;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListMyOrderResponse : ServiceResponseBase
    {
        public MyOrder[] MyOrderList;

        public class MyOrder
        {
            public int MyOrderID;
            public int MyOrderType;
            public string SupplierName;
            public string SellerName;
            public string DateCreated;
            public int Status;
            public string StatusDescription;
            public string Observation;
        }

        #region Methods

        public string Apple()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>");
            sb.Append("<plist version='1.0'>");
            sb.Append("<array>");

            if (this.MyOrderList != null && this.MyOrderList.Count() > 0)
            {
                for (int i = 0; i < this.MyOrderList.Count(); i++)
                {
                    sb.Append("<dict>");

                    sb.Append("     <key>MyOrderID</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].MyOrderID.ToString());
                    sb.Append("     <key>MyOrderType</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].MyOrderType.ToString());
                    sb.Append("     <key>SupplierName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].SupplierName);
                    sb.Append("     <key>SellerName</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].SellerName);
                    sb.Append("     <key>DateCreated</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].DateCreated.ToString());
                    sb.Append("     <key>Status</key>");
                    sb.AppendFormat("     <string>{0}</string>", this.MyOrderList[i].Status.ToString());

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