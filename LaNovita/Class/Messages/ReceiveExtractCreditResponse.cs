using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveExtractCreditListResponse : ServiceResponseBase
    {

        public List<ExtractCredit> ExtractCreditList { get; set; }

        public class ExtractCredit
        {

            public string Date { get; set; } //Format ddMMyyyyHHmm
            public string OperationNumber { get; set; }
            public string Concept { get; set; }
            public int Action { get; set; } //0 debito, 1 Credito
            public double ActionValue { get; set; }
            public double Balance { get; set; }

        }

    }
}