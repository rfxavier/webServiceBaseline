using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListSupplierByAroundResponse : ServiceResponseBase
    {

        public List<SupplierData> SupplierDataList { get; set; }

        public class SupplierData
        {
            public int SupplierId { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string SerialKey { get; set; }
            public string CNPJ { get; set; }
            public decimal ShippingCostRule { get; set; }
            public string Distance { get; set; }
            public int ImageId { get; set; }
            public int ThumbImageId { get; set; }            
            public string DeliveryTime { get; set; }
            public double Rating { get; set; }

        }
       
    }
}