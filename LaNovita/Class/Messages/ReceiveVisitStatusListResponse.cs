using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveVisitStatusListResponse : ServiceResponseBase
    {
        public List<VisitStatus> VisitStatusList { get; set; }
        public class VisitStatus
        {
            public int VisitStatusId { get; set; }
            public string Name { get; set; }
        }
    }
}