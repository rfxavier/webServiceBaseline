using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewOpportunityRequest : ServiceRequestBase
    {
        public string Name { get; set; }
        public string Franchise { get; set; }
        public string Phone { get; set; }
        public int SegmentId { get; set; }
        public string Base64StringImage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Observation { get; set; }
        public int SellerId { get; set; }
    }
}