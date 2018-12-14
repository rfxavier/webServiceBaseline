using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveCustomerCreditRequest : ServiceRequestBase
    {
        public int BuyerId { get; set; }
    }
}