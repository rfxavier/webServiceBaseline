using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCustomerRegistrationRequest : ServiceRequestBase
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int CustomerTypeID { get; set; }
        public string ContributorNumber { get; set; }
        public string ContributorName { get; set; }
        public int CountryId { get; set; }
    }
}