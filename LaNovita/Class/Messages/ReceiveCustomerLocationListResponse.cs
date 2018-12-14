using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCustomerLocationListResponse : ServiceResponseBase
    {
        public List<Location> LocationList { get; set; }

        public class Location
        {
            public int LocationID { get; set; }
            public string Description { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Address { get; set; }
            public int? Number { get; set; }
            public string Complement { get; set; }
            public string Locality { get; set; }
            public int CityID { get; set; }
            public string Reference { get; set; }
            public int ImageId { get; set; }
        }
    }
}