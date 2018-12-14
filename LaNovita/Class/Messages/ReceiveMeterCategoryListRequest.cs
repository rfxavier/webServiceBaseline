using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveMeterCategoryListRequest : ServiceRequestBase
    {
        public string MeterName { get; set; }
    }
}