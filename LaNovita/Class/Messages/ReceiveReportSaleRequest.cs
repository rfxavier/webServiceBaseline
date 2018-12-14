using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveReportSaleRequest : ServiceRequestBase
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}