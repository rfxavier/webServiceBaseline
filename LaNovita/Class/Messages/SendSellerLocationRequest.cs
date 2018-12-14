﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendSellerLocationRequest : ServiceRequestBase
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DateGps { get; set; }
    }
}