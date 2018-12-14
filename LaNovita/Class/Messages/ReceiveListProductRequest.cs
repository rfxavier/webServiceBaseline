using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListProductRequest : ServiceRequestBase
    {
        public int CategoryId { get; set; }
        public int CustomerTypeID { get; set; }
        public int Page { get; set; }
    }
}