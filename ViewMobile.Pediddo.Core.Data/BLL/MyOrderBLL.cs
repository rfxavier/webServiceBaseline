using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class MyOrderBLL
    {
        public MyOrder Save(MyOrder myOrder)
        {
            return new MyOrderDAL().Save(myOrder);
        }

        public void BulkSave(List<MyOrder> myOrderList)
        {
            new MyOrderDAL().BulkSave(myOrderList);
        }

        public MyOrder Update(MyOrder myOrderUpdated)
        {
            MyOrderDAL dal = new MyOrderDAL();
            MyOrder myOrderOrig = dal.GetById(myOrderUpdated.MyOrderID);
            MyOrder myOrder = new MyOrder()
            {
                Active = myOrderUpdated.Active,
                BuyerID = myOrderUpdated.BuyerID,
                DateCreated = myOrderUpdated.DateCreated,
                Description = myOrderUpdated.Description,
                MyOrderID = myOrderUpdated.MyOrderID,
                MyOrderType = myOrderUpdated.MyOrderType,
                Product = myOrderUpdated.Product,
                ReceiptCode = myOrderUpdated.ReceiptCode,
                RepeatOrder = myOrderUpdated.RepeatOrder,
                Sort = myOrderUpdated.Sort,
                SupplierID = myOrderUpdated.SupplierID,
                PromotionID = myOrderUpdated.PromotionID
            };

            return new MyOrderDAL().Update(myOrder, myOrderOrig);
        }

        public void BulkUpdate(List<MyOrder> myOrderUpdated)
        {
            new MyOrderDAL().BulkUpdate(myOrderUpdated);
        }

        public static MyOrder GetById(int myOrderId)
        {
            return new MyOrderDAL().GetById(myOrderId);
        }

        public static MyOrder GetByStatus(int status, int myOrderType, Buyer buyer, int supplierID)
        {
            return MyOrderDAL.GetByStatus(status, myOrderType, buyer, supplierID);
        }

        public static MyOrder GetLast(int myOrderType, Buyer buyer, int supplierID)
        {
            return MyOrderDAL.GetLast(myOrderType, buyer, supplierID);
        }
        public static MyOrder GetLastByBuyer(int myOrderType, Buyer buyer)
        {
            return MyOrderDAL.GetLastByBuyer(myOrderType, buyer);
        }

        public static List<MyOrder> ListByOrderType(int myOrderType, Buyer buyer, int skip)
        {
            return MyOrderDAL.ListByOrderType(myOrderType, buyer, skip);
        }

        public static List<MyOrder> ListByBuyer(Buyer buyer)
        {
            return MyOrderDAL.ListByBuyer(buyer);
        }

        public static List<MyOrder> ListByBuyerByPage(Buyer buyer, int skip)
        {
            return MyOrderDAL.ListByBuyerByPage(buyer, skip);
        }

        public static List<MyOrder> ListBySupplier(Supplier pSupplier, int pStatus)
        {
            return MyOrderDAL.ListBySupplier(pSupplier, pStatus);
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDate, int page)
        {
            return MyOrderDAL.ListBySupplierID(supplierId, statusList, orderTypeList, deliveryDate, page);
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, int dealerId, List<int> statusList, List<int> orderTypeList, int page)
        {


            return MyOrderDAL.ListBySupplierID(supplierId, dealerId, statusList, orderTypeList, page);
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, int dealerId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDateIni, DateTime deliveryDateFin, int page)
        {
            return MyOrderDAL.ListBySupplierID(supplierId, dealerId, statusList, orderTypeList, deliveryDateIni, deliveryDateFin, page);
        }

        public static List<MyOrder> ListByDealerID(int dealerId, List<int> statusList, int page)
        {
            return MyOrderDAL.ListByDealerID(dealerId, statusList, page);
        }

        public static List<MyOrder> ListByDealerID(int dealerId, List<int> statusList, DateTime deliveryDateIni, DateTime deliveryDateFin, int page)
        {
            return MyOrderDAL.ListByDealerID(dealerId, statusList, deliveryDateIni, deliveryDateFin, page);
        }

        public static List<MyOrder> ListBySeller(Seller seller, int status)
        {
            return MyOrderDAL.ListBySeller(seller, status);
        }

        public static List<MyOrder> ListBySeller(Seller seller, int[] statusArray)
        {
            return MyOrderDAL.ListBySeller(seller, statusArray);
        }

        public static List<MyOrder> ListBySeller(Seller seller, int status, int myOrderType)
        {
            return MyOrderDAL.ListBySeller(seller, status, myOrderType);
        }

        public static List<MyOrder> ListBySellerID(int sellerId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDate, int page)
        {
            return MyOrderDAL.ListBySellerID(sellerId, statusList, orderTypeList, deliveryDate, page);
        }

        public static List<MyOrder> ListBySellerAndType(Seller seller, int myOrderType, int month, int year)
        {
            return MyOrderDAL.ListBySellerAndType(seller, myOrderType, month, year);
        }

        public static MyOrder GetFullById(int pMyOrderId)
        {
            return MyOrderDAL.GetFullById(pMyOrderId);
        }

        public static List<MyOrder> ListByCustomerID(int customerID, Seller seller)
        {
            return MyOrderDAL.ListByCustomerID(customerID, seller);
        }

        public static List<MyOrder> ListByBuyerSupplier(int myOrderType, int buyerID, int SupplierID)
        {
            return MyOrderDAL.ListByBuyerSupplier(myOrderType, buyerID, SupplierID);
        }

        public static int GetCountAwaiting(Buyer buyer)
        {
            return MyOrderDAL.GetCountAwaiting(buyer);
        }

        //public List<MyOrder> ListToDealerAdmin()
        //{

        //}

        public static List<MyOrder> ListByDeliveryDate(int supplierID, DateTime deliveryDate, List<int> statusList)
        {
            return MyOrderDAL.ListByDeliveryDate(supplierID, deliveryDate, statusList);
        }

        public static int GetNextDeliveryPosition(int dealerId, DateTime deliveryDate)
        {
            List<int> statusList = new List<int>()
            {
                (int)MyOrderHistoryEnum.Status.Processing,
                (int)MyOrderHistoryEnum.Status.GoingToPoint,
                (int)MyOrderHistoryEnum.Status.Nobody,
                (int)MyOrderHistoryEnum.Status.NotAccepted,
                (int)MyOrderHistoryEnum.Status.Delivered
            };

            return MyOrderDAL.GetNextDeliveryPosition(dealerId, statusList, deliveryDate);

        }

        public static List<MyOrder> ListBetweenPosition(int dealerId, DateTime deliveryDate, int posStart, int posEnd)
        {
            return MyOrderDAL.ListBetweenPosition(dealerId, deliveryDate, posStart, posEnd);
        }

        public static List<MyOrder> ListToInvoiceByName(int supplierID, string query)
        {
            return MyOrderDAL.ListToInvoiceByName(supplierID, query);
        }

        public static List<MyOrder> ListToInvoiceByRuc(int supplierID, string query)
        {
            return MyOrderDAL.ListToInvoiceByRuc(supplierID, query);
        }

        public static MyOrder GetByReceipCode(string receiptCode)
        {
            return MyOrderDAL.GetByReceipCode(receiptCode);
        }

        public static List<MyOrder> ListToPrintInvoice()
        {
            return MyOrderDAL.ListToPrintInvoice();
        }

        public static List<spAdminOrderListResult> ListAdminOrder(int supplierId, int dealerId, string statusList, string orderTypeList, DateTime dateIni, DateTime dateFin, int page)
        {
            return MyOrderDAL.ListAdminOrder(supplierId, dealerId, statusList, orderTypeList, dateIni, dateFin, page);
        }

        public static List<spDealerOrderListResult> ListDealerOrder(int dealerId, string statusList, DateTime dateIni, DateTime dateFin, int page)
        {
            return MyOrderDAL.ListDealerOrder(dealerId, statusList, dateIni, dateFin, page);
        }

        public static List<spSellerOrderListResult> ListSellerOrder(int supplierId, int sellerId, string statusList, string orderTypeList, DateTime deliveryDate, int page)
        {
            return MyOrderDAL.ListSellerOrder(supplierId, sellerId, statusList, orderTypeList, deliveryDate, page);
        }
    }
}
