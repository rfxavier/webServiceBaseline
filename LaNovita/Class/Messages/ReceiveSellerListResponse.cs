using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveSellerListResponse : ServiceResponseBase
    {
        public List<SellerData> SellerDataList { get; set; }
        public class SellerData
        {
            public int SellerId { get; set; }
            public string Name { get; set; }

        }
    }
}