using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveOpportunityListRequest : ServiceRequestBase
    {
        public string Query { get; set; }
        public int Page { get; set; }
    }
}