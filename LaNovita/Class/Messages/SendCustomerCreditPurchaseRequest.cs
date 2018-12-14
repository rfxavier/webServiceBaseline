using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class SendCustomerCreditPurchaseRequest : ServiceRequestBase
    {
        public int PaymentMethod { get; set; }
        public decimal CreditAmount { get; set; }
    }
}