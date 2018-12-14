using System.Collections.Generic;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListSegmentResponse : ServiceResponseBase
    {
        public Customer[] CustomerList;

        public class Customer
        {
            public int CustomerID;
            public string Name;
            public List<Bill> Bills;
        }

        public class Bill
        {
            public int BillID;
            public string ProductName;
            public decimal Value;
            public string Obs;
            public bool Paid;
        }

    }
}