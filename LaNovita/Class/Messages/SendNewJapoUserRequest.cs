using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendNewJapoUserRequest : ServiceRequestBase
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CustomerTypeID { get; set; }
        public string ContributorNumber { get; set; }
        public string ContributorName { get; set; }
        public int CountryId { get; set; }



        public string Description { get; set; }
        public string Address { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string Locality { get; set; }
        public int CityID { get; set; }
        public string Reference { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Base64StringImage { get; set; }

    }
}