using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class DealerBLL
    {
        public Dealer Save(Dealer dealer)
        {
            return new DealerDAL().Save(dealer);
        }

        public Dealer Update(Dealer dealerUpdate, Dealer dealerOrig)
        {
            return new DealerDAL().Update(dealerUpdate, dealerOrig);
        }

        public static Dealer GetById(int dealerId)
        {
            return new DealerDAL().GetById(dealerId);
        }

        public Dealer Authentication(string email, string pPassword)
        {
            return DealerDAL.Authentication(email, pPassword);
        }

        public static List<Dealer> ListBySupplierID(int supplierID)
        {
            return DealerDAL.ListBySupplierID(supplierID);
        }

        public static List<Dealer> ListBySupplierID(int supplierID, int skip)
        {
            return DealerDAL.ListBySupplierID(supplierID, skip);
        }

        public static Dealer GetByEmail(string email)
        {
            return DealerDAL.GetByEmail(email);
        }

        public static Dealer GetBySerialKey(string serialKey)
        {
            return DealerDAL.GetBySerialKey(serialKey);
        }

        public static List<Dealer> ListAdmin(int supplierID)
        {
            return DealerDAL.ListAdmin(supplierID);
        }
    }
}
