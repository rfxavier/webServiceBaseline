using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCustomerListResponse : ServiceResponseBase
    {
        public List<CustomerData> CustomerDataList { get; set; }

        public class CustomerData
        {
            public string Name { get; set; }
            public string Segment { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string ContributorNumber { get; set; }
            public string ContributorName { get; set; }
            public int BuyerId { get; set; }
            public int CountryId { get; set; }
            public int CustomerTypeId { get; set; }
            public List<Location> LocationList { get; set; }

            public class Location
            {
                public int LocationId { get; set; }
                public string Description { get; set; }
                public double Latitude { get; set; }
                public double Longitude { get; set; }
                public string Address { get; set; }
                public int? Number { get; set; }
                public string Complement { get; set; }
                public string Locality { get; set; }
                public int CityId { get; set; }
                public string Reference { get; set; }
                public int ImageId { get; set; }
            }

        }
    }
}