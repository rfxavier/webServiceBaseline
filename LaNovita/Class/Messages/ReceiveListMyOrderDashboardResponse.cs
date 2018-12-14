using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ViewMobile.Pediddo.WebService.Mobile.Messages
{
    public class ReceiveListMyOrderDashboardResponse : ServiceResponseBase
    {
        public List<MyOrder> MyOrderListDashboard;

        public class MyOrder
        {
            public int Status;
            public string StatusDescription;
            public string StatusColor;
            public int Count;
        }
    }
}