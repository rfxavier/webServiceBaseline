using System.Collections.Generic;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SupplierBLL
    {
        public Supplier Save(Supplier supplier)
        {
            return new SupplierDAL().Save(supplier);
        }

        public Supplier Update(Supplier supplierUpdate, Supplier supplierOrigin)
        {
            return new SupplierDAL().Update(supplierUpdate, supplierOrigin);
        }

        public static List<Supplier> ListByBuyer(Buyer buyer)
        {
            return SupplierDAL.ListByBuyer(buyer);
        }

        public static List<Supplier> ListByBuyerByPage(Buyer buyer, int skip)
        {
            return SupplierDAL.ListByBuyerByPage(buyer, skip);
        }

        public static List<Supplier> ListByAround()
        {
            return SupplierDAL.ListByAround();
        }

        public static Supplier GetBySerialKey(string serialKey)
        {
            return SupplierDAL.GetBySerialKey(serialKey);
        }

    }
}
