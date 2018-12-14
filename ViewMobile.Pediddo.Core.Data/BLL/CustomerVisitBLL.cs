using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CustomerVisitBLL
    {
        public CustomerVisit GetById(int id)
        {
            return new CustomerVisitDAL().GetById(id);
        }

        public CustomerVisit Save(CustomerVisit customerVisit)
        {
            return new CustomerVisitDAL().Save(customerVisit);
        }

        public static CustomerVisit GetLastByCustomerID(int customerId)
        {
            return CustomerVisitDAL.GetLastByCustomerID(customerId);
        }

        public CustomerVisit Update(CustomerVisit customerVisitUpdated)
        {
            CustomerVisitDAL customerVisitDAL = new CustomerVisitDAL();
            CustomerVisit customerVisitOrig = customerVisitDAL.GetById(customerVisitUpdated.CustomerVisitID);

            CustomerVisit customerVisit = new CustomerVisit()
            {
                Active = customerVisitUpdated.Active,
                ChiefName = customerVisitUpdated.ChiefName,
                ChiefPhone = customerVisitUpdated.ChiefPhone,
                ContactName = customerVisitUpdated.ContactName,
                ContactPhone = customerVisitUpdated.ContactPhone,
                CustomerID = customerVisitUpdated.CustomerID,
                CustomerVisitID = customerVisitUpdated.CustomerVisitID,
                DateCreated = customerVisitUpdated.DateCreated,
                Description = customerVisitUpdated.Description,
                SellerID = customerVisitUpdated.SellerID,
                VisitEnd = customerVisitUpdated.VisitEnd,
                VisitStart = customerVisitUpdated.VisitStart
            };


            return customerVisitDAL.Update(customerVisit, customerVisitOrig);
        }

        public static List<CustomerVisit> ListBySeller(int sellerId, int month, int year)
        {
            return CustomerVisitDAL.ListBySeller(sellerId, month, year);
        }
    }
}
