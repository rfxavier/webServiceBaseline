using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCreateOrderInteractionRequest :  ServiceRequestBase
    {
        public int OrderId { get; set; }
        public int ResultId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Base64StringImage { get; set; }
        public string Observation { get; set; }
        public string Receiver { get; set; }
    }
}