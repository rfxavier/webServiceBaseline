using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class BuyerBLL
    {
        public static Buyer GetById(int buyerId)
        {
            return new BuyerDAL().GetById(buyerId);
        }

        public Buyer Update(Buyer BuyerToUpdate, Buyer BuyerOrig)
        {
            return new BuyerDAL().Update(BuyerToUpdate, BuyerOrig);
        }

        public Buyer Save(Buyer buyer)
        {
            return new BuyerDAL().Save(buyer);
        }
        public Buyer Authentication(string pEmail, string pPassword)
        {
            return BuyerDAL.Authentication(pEmail, pPassword);
        }

        public static Buyer HasEmail(string eMail)
        {
            return BuyerDAL.HasEmail(eMail);
        }

        public static Buyer GetBySerialKey(string pSerialKey)
        {
            return BuyerDAL.GetBySerialKey(pSerialKey);
        }

        public static List<Buyer> GetBySellerID(int sellerID)
        {
            return BuyerDAL.GetBySellerID(sellerID);
        }

        public static List<Buyer> GetBySellerCustomer(int sellerID, int customerID)
        {
            return BuyerDAL.GetBySellerCustomer(sellerID, customerID);
        }

        public static List<Buyer> GetByCustomer(int customerID)
        {
            return BuyerDAL.GetByCustomer(customerID);
        }

        public Buyer Update(Buyer buyerUpdated)
        {
            BuyerDAL dal = new BuyerDAL();
            Buyer buyerOrig = dal.GetById(buyerUpdated.BuyerID);
            Buyer buyer = new Buyer()
            {
                Active = buyerUpdated.Active,
                Admin = buyerUpdated.Admin,
                BuyerID = buyerUpdated.BuyerID,
                CustomerID = buyerUpdated.CustomerID,
                DateCreated = buyerUpdated.DateCreated,
                DocumentNumber = buyerUpdated.DocumentNumber,
                Email = buyerUpdated.Email,
                FirstName = buyerUpdated.FirstName,
                IsBlocked = buyerUpdated.IsBlocked,
                LastName = buyerUpdated.LastName,
                Password = buyerUpdated.Password,
                PhoneNumber = buyerUpdated.PhoneNumber,
                SerialKey = buyerUpdated.SerialKey,
                AppImageId = buyerUpdated.AppImageId
            };

            return dal.Update(buyer, buyerOrig);
        }

        public static List<Buyer> ListCustomerVip()
        {
            return BuyerDAL.ListCustomerVip();
        }

        public static List<Buyer> ListCustomerWithCredit()
        {
            return BuyerDAL.ListCustomerWithCredit();
        }
    }
}
