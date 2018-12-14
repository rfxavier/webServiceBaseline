using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListDealerResponse : ServiceResponseBase
    {
        public List<Dealer> DealerList { get; set; }

        public class Dealer
        {
            public string Name { get; set; }
            public int DealerId { get; set; }
        }

    }
}