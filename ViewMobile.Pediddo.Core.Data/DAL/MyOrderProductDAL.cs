using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class MyOrderProductDAL : DAOBase<MyOrderProduct, int>
    {
        public static MyOrderProduct GetById(int myOrderID, int productID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.MyOrderProducts.FirstOrDefault(op => op.MyOrderID == myOrderID && op.ProductID == productID && op.Active == true);
            }
        }

        public static List<MyOrderProduct> GetByMyOrderID(int myOrderID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.MyOrderProducts.Where(op => op.MyOrderID == myOrderID && op.Active == true).ToList();
            }
        }

        public void BulkSave(List<MyOrderProduct> pMyOrderProductList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (MyOrderProduct myOrderProduct in pMyOrderProductList)
                    {
                        db.MyOrderProducts.InsertOnSubmit(myOrderProduct);
                        db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    scope.Complete();
                }
            }
        }

        public void Delete(int myOrderID, int productID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                MyOrderProduct myOrderProductToDelete = db.MyOrderProducts.FirstOrDefault(op => op.MyOrderID == myOrderID && op.ProductID == productID && op.Active == true);
                db.MyOrderProducts.DeleteOnSubmit(myOrderProductToDelete);
                db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
            }
        }

        public void BulkUpdate(List<MyOrderProduct> myOrderProductUpdatedList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    MyOrderProduct _MyOrderProduct;
                    foreach (MyOrderProduct myOrderProductUpdated in myOrderProductUpdatedList)
                    {
                        _MyOrderProduct = db.MyOrderProducts.FirstOrDefault(h => h.MyOrderID == myOrderProductUpdated.MyOrderID && h.ProductID == myOrderProductUpdated.ProductID);

                        _MyOrderProduct.Active = myOrderProductUpdated.Active;
                        _MyOrderProduct.Cost = myOrderProductUpdated.Cost;
                        _MyOrderProduct.DateCreated = myOrderProductUpdated.DateCreated;
                        _MyOrderProduct.Discount = myOrderProductUpdated.Discount;
                        _MyOrderProduct.DiscountInitial = myOrderProductUpdated.DiscountInitial;
                        _MyOrderProduct.IsAdded = myOrderProductUpdated.IsAdded;
                        _MyOrderProduct.MyOrderID = myOrderProductUpdated.MyOrderID;
                        _MyOrderProduct.ProductID = myOrderProductUpdated.ProductID;
                        _MyOrderProduct.Quantity = myOrderProductUpdated.Quantity;
                        _MyOrderProduct.QuantityInitial = myOrderProductUpdated.QuantityInitial;
                        _MyOrderProduct.PriceListID = myOrderProductUpdated.PriceListID;
                        _MyOrderProduct.SalePrice = myOrderProductUpdated.SalePrice;
                        _MyOrderProduct.InOffer = myOrderProductUpdated.InOffer;
                        
                        db.SubmitChanges();
                    }
                    scope.Complete();
                }
            }
        }

        public void BulkDelete(List<MyOrderProduct> myOrderProductList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {

                    MyOrderProduct _MyOrderProduct = null;
                    foreach (MyOrderProduct myOrderProduct in myOrderProductList)
                    {
                        _MyOrderProduct = new MyOrderProduct();
                        _MyOrderProduct.Active = myOrderProduct.Active;
                        _MyOrderProduct.Cost = myOrderProduct.Cost;
                        _MyOrderProduct.DateCreated = myOrderProduct.DateCreated;
                        _MyOrderProduct.Discount = myOrderProduct.Discount;
                        _MyOrderProduct.DiscountInitial = myOrderProduct.DiscountInitial;
                        _MyOrderProduct.IsAdded = myOrderProduct.IsAdded;
                        _MyOrderProduct.MyOrderID = myOrderProduct.MyOrderID;
                        _MyOrderProduct.ProductID = myOrderProduct.ProductID;
                        _MyOrderProduct.Quantity = myOrderProduct.Quantity;
                        _MyOrderProduct.QuantityInitial = myOrderProduct.QuantityInitial;
                        _MyOrderProduct.PriceListID = myOrderProduct.PriceListID;
                        _MyOrderProduct.SalePrice = myOrderProduct.SalePrice;
                        _MyOrderProduct.InOffer = myOrderProduct.InOffer;

                        db.MyOrderProducts.Attach(_MyOrderProduct);
                        db.MyOrderProducts.DeleteOnSubmit(_MyOrderProduct);
                        db.SubmitChanges();
                    }
                    scope.Complete();
                }
            }
        }
    }
}
