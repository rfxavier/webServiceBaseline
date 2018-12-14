using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendUpdateSellerCustomerRequest : ServiceRequestBase
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string BusinessDocument { get; set; }
        public string Franchise { get; set; }
        public string Phone { get; set; }
        public int SegmentId { get; set; }
        public int Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Observation { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ChiefName { get; set; }
        public string ChiefPhone { get; set; }
        public string Email { get; set; }
    }
}