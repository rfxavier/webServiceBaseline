using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveContentToShareResponse : ServiceResponseBase
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public int ImageId { get; set; }
    }
}