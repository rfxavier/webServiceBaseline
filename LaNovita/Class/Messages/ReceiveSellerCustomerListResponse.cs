using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveSellerCustomerListResponse : ServiceResponseBase
    {
        public List<SellerCustomer> SellerCustomerList { get; set; }
        public class SellerCustomer
        {
            public int CustomerID { get; set; }
            public string Name { get; set; }
            public string BusinessName { get; set; }
            public string BusinessDocument { get; set; }
            public string Franchise { get; set; }
            public string Phone { get; set; }
            public int SegmentId { get; set; }
            public string SegmentName { get; set; }
            public int Status { get; set; }
            public string StatusName { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Observation { get; set; }
            public string ContactName { get; set; }
            public string ContactPhone { get; set; }
            public string ChiefName { get; set; }
            public string ChiefPhone { get; set; }
            public string Email { get; set; }
            public string SellerName { get; set; }
            public string SellerPhone { get; set; }
        }
    }
}