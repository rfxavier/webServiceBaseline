using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCalculateShippingCostResponse : ServiceResponseBase
    {
        public decimal ShippingCost { get; set; }
        public decimal FreeShippingAmount { get; set; }
    }
}