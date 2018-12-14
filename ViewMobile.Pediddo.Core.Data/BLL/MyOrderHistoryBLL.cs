using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class MyOrderHistoryBLL
    {
        public MyOrderHistory Save(MyOrderHistory myOrderHistory)
        {
            return new MyOrderHistoryDAL().Save(myOrderHistory);
        }

        public static List<MyOrderHistory> GetByMyOrderID(int pMyOrderID)
        {
            return MyOrderHistoryDAL.GetByMyOrderID(pMyOrderID);
        }

        public static MyOrderHistory GetLast(int myOrderID)
        {
            return MyOrderHistoryDAL.GetLast(myOrderID);
        }
    }
}
