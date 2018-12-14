using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveOrderListRequest : ServiceRequestBase
    {
        public int Page { get; set; }
        public int Option { get; set; }
        public string DeliveryDate { get; set; }
    }
}