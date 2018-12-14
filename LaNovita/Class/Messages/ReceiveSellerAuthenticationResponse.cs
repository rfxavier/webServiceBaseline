using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveSellerAuthenticationResponse : ServiceResponseBase
    {
        public string Name { get; set; }
        public int SellerID { get; set; }
        public int SupplierID { get; set; }
        public string SerialKey { get; set; }
        public string NotificationEndpoint { get; set; }
        public bool IsAdmin { get; set; }
        public string WorkingDayStartDate { get; set; }
    }
}