using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveReportSaleResponse : ServiceResponseBase
    {
        public List<ReportSale> ReportSaleList { get; set; }
        public class ReportSale
        {
            public int Type { get; set; }
            public decimal Amount { get; set; }
        }
    }
}