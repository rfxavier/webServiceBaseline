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
    public class BuyerSellerDAL : DAOBase<BuyerSeller, int>
    {
        public static BuyerSeller GetById(int buyerId, int sellerId)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.BuyerSellers.FirstOrDefault(bs => bs.BuyerID == buyerId && bs.SellerID == sellerId);
            }
        }

        public static List<BuyerSeller> ListByBuyerAndSupplier(int buyerID, int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<BuyerSeller>(p => p.Seller);
                dlo.LoadWith<Seller>(p => p.Employee);
                dlo.LoadWith<Employee>(p => p.Supplier);
                db.LoadOptions = dlo;

                return db.BuyerSellers.Where(bs => bs.BuyerID == buyerID && bs.Seller.Employee.SupplierID == supplierID).ToList();
            }
        }

        public void BulkDelete(List<BuyerSeller> pBuyerSellerList)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                using (var scope = new TransactionScope())
                {
                    foreach (BuyerSeller buyerSeller in pBuyerSellerList)
                    {
                        db.BuyerSellers.Attach(buyerSeller);
                        db.BuyerSellers.DeleteOnSubmit(buyerSeller);
                        db.SubmitChanges();
                    }
                    scope.Complete();
                }
            }
        }

        public void DeleteBuyerSeller(int buyerID, int sellerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.ExecuteCommand($"delete from buyerseller where sellerid = {sellerID.ToString()} and buyerid = {buyerID.ToString()}");
            }
        }


    }
}
