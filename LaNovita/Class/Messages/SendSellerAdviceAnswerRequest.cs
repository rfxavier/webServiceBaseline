﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendSellerAdviceAnswerRequest : ServiceRequestBase
    {
        public int AdviceId { get; set; }
        public int Answer { get; set; }
    }
}