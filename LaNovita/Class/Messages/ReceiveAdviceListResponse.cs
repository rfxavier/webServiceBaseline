using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveAdviceListResponse : ServiceResponseBase
    {
        public List<Advice> AdviceList { get; set; }

        public class Advice
        {
            public int AdviceId { get; set; }
            public string Title { get; set; }
            public string Call { get; set; }
            public string Body { get; set; }
            public int ImageId { get; set; }
            public List<AdviceButton> AdviceButtonList { get; set; }
            public string Date { get; set; }
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }

            public class AdviceButton
            {
                public int Id { get; set; }
                public string Text { get; set; }
            }
        }
    }
}