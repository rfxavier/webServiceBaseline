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
    public class SendNewMyOrderResponse : ServiceResponseBase
    {
        public NewOrder[] NewOrderList;

        public class NewOrder
        {
            public int MyOrderID;
            public string ReceiptCode;
        }
    }
}