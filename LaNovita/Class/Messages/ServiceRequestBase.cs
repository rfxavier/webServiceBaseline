using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ServiceRequestBase
    {
        public string SerialKey { get; set; }
        public string UUID { get; set; }
        public string Locale { get; set; }
        public int AppID { get; set; }
    }
}