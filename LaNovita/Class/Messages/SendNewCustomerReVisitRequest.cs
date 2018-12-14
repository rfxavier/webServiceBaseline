using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewCustomerReVisitRequest : ServiceRequestBase
    {
        public int CustomerId { get; set; }
        public int ReVisitResultId { get; set; }
        public string NextStep { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}