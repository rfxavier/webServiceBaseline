using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class MyOrderProductBLL
    {
        public static MyOrderProduct GetById(int pMyOrderID, int pProductID)
        {
            return MyOrderProductDAL.GetById(pMyOrderID, pProductID);
        }

        public static List<MyOrderProduct> GetByMyOrderID(int pMyOrderID)
        {
            return MyOrderProductDAL.GetByMyOrderID(pMyOrderID);
        }
        public void BulkSave(List<MyOrderProduct> myOrderProductList)
        {
            new MyOrderProductDAL().BulkSave(myOrderProductList);
        }

        public MyOrderProduct Update(MyOrderProduct pMyOrderProductUpdate, MyOrderProduct pMyOrderProductOrig)
        {
            return new MyOrderProductDAL().Update(pMyOrderProductUpdate, pMyOrderProductOrig);
        }

        public void Delete(int myOrderID, int productID)
        {
            new MyOrderProductDAL().Delete(myOrderID, productID);
        }

        public MyOrderProduct Save(MyOrderProduct myOrderProduct)
        {
            return new MyOrderProductDAL().Save(myOrderProduct);
        }

        public void BulkUpdate(List<MyOrderProduct> myOrderProductListUpdated)
        {
            new MyOrderProductDAL().BulkUpdate(myOrderProductListUpdated);
        }

        public void BulkDelete(List<MyOrderProduct> myOrderProductList)
        {
            new MyOrderProductDAL().BulkDelete(myOrderProductList);
        }
    }
}
