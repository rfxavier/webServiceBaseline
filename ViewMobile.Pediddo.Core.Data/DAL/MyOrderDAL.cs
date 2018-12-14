using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Enumeration;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class MyOrderDAL : DAOBase<MyOrder, int>
    {
        public void BulkSave(List<MyOrder> pMyOrderList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (MyOrder myOrder in pMyOrderList)
                    {
                        db.MyOrders.InsertOnSubmit(myOrder);
                        db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    scope.Complete();
                }
            }
        }

        public static MyOrder GetByStatus(int pStatus, int pMyOrderType, Buyer pBuyer, int pSupplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.FirstOrDefault(c => c.BuyerID == pBuyer.BuyerID &&
                                                 c.SupplierID == pSupplierID &&
                                                 c.MyOrderHistories.OrderByDescending(o=>o.DateCreated).FirstOrDefault().Status == pStatus &&
                                                 c.MyOrderType == pMyOrderType &&
                                                 c.Active == true);
            }
        }

        public static MyOrder GetLast(int pMyOrderType, Buyer pBuyer, int pSupplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<Product>(p => p.UnitMeasure);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                                 c.SupplierID == pSupplierID &&
                                                 c.MyOrderType == pMyOrderType &&
                                                 c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == (int)MyOrderHistoryEnum.Status.Executed &&
                                                 c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .FirstOrDefault();
            }
        }

        public static MyOrder GetLastByBuyer(int pMyOrderType, Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<Product>(p => p.UnitMeasure);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.MyOrderType == pMyOrderType &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == (int)MyOrderHistoryEnum.Status.Executed &&
                                              c.Active == true)
                    .OrderByDescending(o => o.DateCreated)
                    .FirstOrDefault();
            }
        }

        public static List<MyOrder> ListByOrderType(int pMyOrderType, Buyer pBuyer, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.MyOrderType == pMyOrderType &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .Skip(pSkip)
                                  .Take(10)
                                  .ToList();
            }
        }

        public static List<MyOrder> ListByBuyer(Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .ToList();
            }
        }

        public static List<MyOrder> ListByBuyerByPage(Buyer pBuyer, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .Skip(pSkip)
                                  .Take(10)
                                  .ToList();
            }
        }

        public static List<MyOrder> ListBySupplier(Supplier pSupplier, int pStatus)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.Supplier.SupplierID == pSupplier.SupplierID &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == pStatus &&
                                              c.Active == true)
                                  .OrderBy(o => o.DateCreated)
                                  .ToList();
            }
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDate, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.Dealer);
                dlo.LoadWith<Dealer>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (orderTypeList.Contains(c.MyOrderType.Value)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t => t.Sort)
                                  .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (orderTypeList.Contains(c.MyOrderType.Value)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t => t.Sort)
                                  .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, int dealerId, List<int> statusList, List<int> orderTypeList, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (c.MyOrderType.Value == (int)MyOrderEnum.Type.Order || (orderTypeList.Contains(c.MyOrderType.Value) && c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId)) &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t=>t.Sort)
                                  .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (c.MyOrderType.Value == (int)MyOrderEnum.Type.Order || (orderTypeList.Contains(c.MyOrderType.Value) && c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId)) &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t => t.Sort)
                                  .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListBySupplierID(int supplierId, int dealerId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDateIni, DateTime deliveryDateFin, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (c.MyOrderType.Value == (int)MyOrderEnum.Type.Order || (orderTypeList.Contains(c.MyOrderType.Value) && c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date >= deliveryDateIni &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date <= deliveryDateFin &&
                                              c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                  .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (c.MyOrderType.Value == (int)MyOrderEnum.Type.Order || (orderTypeList.Contains(c.MyOrderType.Value) && c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date >= deliveryDateIni &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date <= deliveryDateFin &&
                                              c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                  .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListByDealerID(int dealerId, List<int> statusList, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                                  statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                                  c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                      .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                   return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                      .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListByDealerID(int dealerId, List<int> statusList, DateTime deliveryDateIni, DateTime deliveryDateFin, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                                  statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                                  c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date >= deliveryDateIni &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date <= deliveryDateFin &&
                                                  c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                      .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                               statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                               c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date >= deliveryDateIni &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date <= deliveryDateFin &&
                                               c.Active == true)
                                      .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                      .ThenBy(t => t.Sort)
                                       .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListBySeller(Seller pSeller, int pStatus)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(a => a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == pStatus &&
                                                  a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == pSeller.SellerID &&
                                                  a.Active == true)
                                                  .OrderBy(o => o.DateCreated).ToList();
            }
        }

        public static List<MyOrder> ListBySeller(Seller pSeller, int[] pStatusArray)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(a => pStatusArray.Contains(a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                                  a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == pSeller.SellerID &&
                                                  a.MyOrderType != (int)MyOrderEnum.Type.CallMe &&
                                                  a.Active == true
                                                  )
                                                  .OrderBy(o => o.DateCreated).ToList();
            }
        }

        public static List<MyOrder> ListBySellerID(int sellerId, List<int> statusList, List<int> orderTypeList, DateTime deliveryDate, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.Dealer);
                dlo.LoadWith<Dealer>(p => p.Employee);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                if (page == 0)
                {
                    return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == sellerId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (orderTypeList.Contains(c.MyOrderType.Value)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t => t.Sort)
                                  .ToList();
                }
                else
                {
                    int skip = (Convert.ToInt32(page) - 1) * 10;
                    return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == sellerId &&
                                              statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                              (orderTypeList.Contains(c.MyOrderType.Value)) &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                              c.Active == true)
                                  .OrderBy(d => d.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate)
                                  .ThenBy(t => t.Sort)
                                  .Skip(skip).Take(10).ToList();
                }
            }
        }

        public static List<MyOrder> ListBySeller(Seller pSeller, int pStatus, int pMyOrderType)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(a => a.MyOrderType == pMyOrderType &&
                                                  a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == pStatus &&
                                                  a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == pSeller.SellerID &&
                                                  a.Active == true)
                                                  .OrderBy(o=> o.DateCreated).ToList();
            }
        }

        public static List<MyOrder> ListBySellerAndType(Seller pSeller, int pMyOrderType, int month, int year)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(a => a.MyOrderType == pMyOrderType &&
                                            a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == pSeller.SellerID &&
                                            a.DateCreated.Value.Month == month &&
                                            a.DateCreated.Value.Year == year &&
                                            a.Active == true)
                                            .OrderBy(o => o.DateCreated).ToList();
            }
        }

        public static MyOrder GetFullById(int myOrderId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                db.LoadOptions = dlo;

                return db.MyOrders.FirstOrDefault(a => a.MyOrderID == myOrderId);
            }
        }

        public static List<MyOrder> ListByCustomerID(int pCustomerID, Seller pSeller)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(a => a.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().SellerID == pSeller.SellerID &&
                                              a.Buyer.Customer.CustomerID == pCustomerID &&
                                                  a.Active == true)
                                                  .OrderByDescending(o => o.DateCreated).ToList();
            }
        }

        public static List<MyOrder> ListByBuyerSupplier(int pMyOrderType, int pBuyerID, int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.MyOrderType == pMyOrderType &&
                                              c.BuyerID == pBuyerID &&
                                              c.SupplierID == supplierID &&
                                              c.Active == true)
                                  .OrderByDescending(o => o.DateCreated)
                                  .ToList();
            }
        }

        public static int GetCountAwaiting(Buyer pBuyer)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.BuyerID == pBuyer.BuyerID &&
                                              c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status == (int)MyOrderHistoryEnum.Status.AwaitingAttention &&
                                              c.Active == true)
                                  .ToList().Count();
            }
        }

        public static List<MyOrder> ListByDeliveryDate(int supplierId, DateTime deliveryDate, List<int> statusList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.Supplier.SupplierID == supplierId &&
                                            statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                            c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                            c.Active == true)
                                .OrderBy(o => o.DateCreated)
                                .ToList();
                
            }
        }

        public static List<MyOrder> ListBetweenPosition(int dealerId, DateTime deliveryDate, int posStart, int posEnd)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Supplier);
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                            c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate &&
                                            c.Sort >= posStart &&
                                            c.Sort <= posEnd &&
                                            c.Active == true)
                                .OrderBy(o => o.Sort)
                                .ToList();
            }
        }

        public void BulkUpdate(List<MyOrder> myOrderUpdatedList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    MyOrder _MyOrder;
                    foreach (MyOrder myOrderUpdated in myOrderUpdatedList)
                    {
                        _MyOrder = db.MyOrders.FirstOrDefault(h => h.MyOrderID == myOrderUpdated.MyOrderID);

                        _MyOrder.Active = myOrderUpdated.Active;
                        _MyOrder.BuyerID = myOrderUpdated.BuyerID;
                        _MyOrder.DateCreated = myOrderUpdated.DateCreated;
                        _MyOrder.Description = myOrderUpdated.Description;
                        _MyOrder.MyOrderID = myOrderUpdated.MyOrderID;
                        _MyOrder.MyOrderType = myOrderUpdated.MyOrderType;
                        _MyOrder.Product = myOrderUpdated.Product;
                        _MyOrder.ReceiptCode = myOrderUpdated.ReceiptCode;
                        _MyOrder.RepeatOrder = myOrderUpdated.RepeatOrder;
                        _MyOrder.Sort = myOrderUpdated.Sort;
                        _MyOrder.SupplierID = myOrderUpdated.SupplierID;
                        _MyOrder.PromotionID = myOrderUpdated.PromotionID;

                        db.SubmitChanges();
                    }
                    scope.Complete();
                }
            }
        }

        public static List<MyOrder> ListToInvoiceByName(int supplierID, string query)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => 
                                            c.MyOrderHistories.Where(o=>o.InteractionID == (int)MyOrderHistoryEnum.InteractionID.Invoicing).FirstOrDefault() == null &&
                                            (
                                                c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().InvoiceNumber == null ||
                                                c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().InvoiceNumber.Trim() == ""
                                            )&&
                                            c.SupplierID == supplierID &&
                                            (
                                                ((c.Buyer.FirstName ?? "") + " " + (c.Buyer.LastName ?? "")).Contains(query) ||
                                                c.Buyer.Customer.Name.Contains(query)
                                            ) &&
                                            c.Active == true)
                                .OrderByDescending(o => o.DateCreated)
                                .ToList();
            }
        }

        public static List<MyOrder> ListToInvoiceByRuc(int supplierID, string query)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c =>
                                            c.MyOrderHistories.Where(o => o.InteractionID == (int)MyOrderHistoryEnum.InteractionID.Invoicing).FirstOrDefault() == null &&
                                            (
                                                c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().InvoiceNumber == null ||
                                                c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().InvoiceNumber.Trim() == ""
                                            ) &&
                                            c.SupplierID == supplierID &&
                                            c.Buyer.Customer.CNPJ.Contains(query) &&
                                            c.Active == true)
                                .OrderByDescending(o => o.DateCreated)
                                .ToList();
            }
        }

        public static MyOrder GetByReceipCode(string receiptCode)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.MyOrders.FirstOrDefault(c => c.ReceiptCode.Equals(receiptCode));
            }
        }

        public static List<MyOrder> ListToPrintInvoice()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<MyOrder>(p => p.Buyer);
                dlo.LoadWith<Buyer>(p => p.Customer);
                dlo.LoadWith<MyOrder>(p => p.MyOrderHistories);
                dlo.LoadWith<MyOrderHistory>(p => p.CustomerLocation);
                dlo.LoadWith<MyOrder>(p => p.MyOrderProducts);
                dlo.LoadWith<MyOrderProduct>(p => p.Product);
                dlo.LoadWith<MyOrderHistory>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                db.LoadOptions = dlo;

                return db.MyOrders.Where(c => (c.PrintInvoice ?? false)).ToList();
            }
        }

        public static int GetNextDeliveryPosition(int dealerId, List<int> statusList, DateTime deliveryDate)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                List<MyOrder> myOrderList =
                db.MyOrders.Where(c => c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DealerID == dealerId &&
                                       statusList.Contains(c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().Status.Value) &&
                                       c.MyOrderHistories.OrderByDescending(o => o.DateCreated).FirstOrDefault().DeliveryDate.Value.Date == deliveryDate.Date &&
                                       c.Active == true)
                                       .ToList();

                int maxPosition = myOrderList.Max(mo => mo.Sort) ?? 0;
                return (maxPosition + 1);
            }

        }

        public static List<spAdminOrderListResult> ListAdminOrder(int supplierId, int dealerId, string statusList, string orderTypeList, DateTime dateIni, DateTime dateFin, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {

                return db.spAdminOrderList(supplierId, dealerId, statusList, orderTypeList, dateIni, dateFin, page).ToList();
            }
        }

        public static List<spDealerOrderListResult> ListDealerOrder(int dealerId, string statusList, DateTime dateIni, DateTime dateFin, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.spDealerOrderList(dealerId, statusList, dateIni, dateFin, page).ToList();
            }
        }

        public static List<spSellerOrderListResult> ListSellerOrder(int supplierId, int sellerId, string statusList, string orderTypeList, DateTime deliveryDate, int page)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.spSellerOrderList(supplierId, sellerId, statusList, orderTypeList, deliveryDate, page).ToList();
            }
        }
    }
}
