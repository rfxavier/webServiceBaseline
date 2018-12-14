using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveOpportunityListResponse : ServiceResponseBase
    {
        public List<Opportunity> OpportunityList { get; set; }
        public class Opportunity
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Franchise { get; set; }
            public int SegmentID { get; set; }
            public string SegmentName { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Observation { get; set; }
            public int ImageId { get; set; }
            public int ThumbImageId { get; set; }
        }
    }
}