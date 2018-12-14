using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendAdjustCustomerLocationRequest : ServiceRequestBase
    {
        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Base64StringImage { get; set; }
    }
}