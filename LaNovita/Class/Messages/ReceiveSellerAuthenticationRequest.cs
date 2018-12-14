using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveSellerAuthenticationRequest : ServiceRequestBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int DeviceOS { get; set; }

    }
}