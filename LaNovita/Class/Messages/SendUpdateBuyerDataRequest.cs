using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendUpdateBuyerDataRequest : ServiceRequestBase
    {
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ContributorName { get; set; }
        public string ContributorNumber { get; set; }
        public int CountryId { get; set; }
    }
}