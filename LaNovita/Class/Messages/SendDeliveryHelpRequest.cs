using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendDeliveryHelpRequest : ServiceRequestBase
    {
        public int OrderId { get; set; }
    }
}