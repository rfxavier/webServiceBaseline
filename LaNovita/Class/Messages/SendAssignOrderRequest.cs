using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendAssignOrderRequest : ServiceRequestBase
    {
        public int DealerId { get; set; }
        public int OrderId { get; set; }
    }
}