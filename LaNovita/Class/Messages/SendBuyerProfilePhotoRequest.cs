using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendBuyerProfilePhotoRequest : ServiceRequestBase
    {
        public string Base64StringImage { get; set; }
    }
}