using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCalculateShippingCostRequest : ServiceRequestBase
    {
        public int LocationID { get; set; }
    }
}