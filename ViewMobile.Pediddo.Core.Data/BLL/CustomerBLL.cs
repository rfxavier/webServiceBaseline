using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CustomerBLL
    {
        public Customer Save(Customer customer)
        {
            return new CustomerDAL().Save(customer);
        }

        public static Customer GetByCNPJ(string Cnpj)
        {
            return CustomerDAL.GetByCNPJ(Cnpj);
        }

        public static Customer GetById(int customerId)
        {
            return new CustomerDAL().GetById(customerId);
        }

        public static List<Customer> GetByName(string Name)
        {
            return CustomerDAL.GetByName(Name);
        }

        public static List<Customer> GetBySellerID(int sellerID)
        {
            return CustomerDAL.GetBySellerID(sellerID);
        }
        public static List<Customer> GetCustomerSync(DateTime LastDateTime)
        {
            return CustomerDAL.GetCustomerSync(LastDateTime);
        }

        public static List<Customer> GetBySellerID(int sellerID, string query, int page)
        {
            int skip = (Convert.ToInt32(page) - 1) * 20;
            return CustomerDAL.GetBySellerID(sellerID, query, skip);
        }

        public static List<string> ListCustomer(string prefixText, int count, int sellerId)
        {
            using (DataClasses1DataContext repositorio = new DataClasses1DataContext())
            {
                var _result = (from lista in repositorio.Customers
                               where lista.Name.StartsWith(prefixText.Replace("'", "''")) && lista.Buyers.Any(b => b.BuyerSellers.Any(bs => bs.SellerID == sellerId))
                               select new { Name = lista.Name}).Take(count);

                List<string> t = new List<string>();

                foreach (var tt in _result)
                {
                    t.Add(tt.Name.ToUpperInvariant());
                }
                return t;
            }
        }

        public Customer Update(Customer customerUpdated)
        {
            CustomerDAL dal = new CustomerDAL();
            Customer customerOrig = dal.GetById(customerUpdated.CustomerID);

            Customer customer = new Customer()
            {
                Active = customerUpdated.Active,
                Address = customerUpdated.Address,
                BusinessName = customerUpdated.BusinessName,
                CNPJ = customerUpdated.CNPJ,
                CustomerID = customerUpdated.CustomerID,
                DateCreated = customerUpdated.DateCreated,
                Latitude = customerUpdated.Latitude,
                Longitude = customerUpdated.Longitude,
                Name = customerUpdated.Name,
                Observation = customerUpdated.Observation,
                PhoneNumber = customerUpdated.PhoneNumber,
                SegmentID = customerUpdated.SegmentID,
                Status = customerUpdated.Status,
                Franchise = customerUpdated.Franchise,
                ContactName = customerUpdated.ContactName,
                ContactPhone = customerUpdated.ContactPhone,
                ChiefName = customerUpdated.ChiefName,
                ChiefPhone = customerUpdated.ChiefPhone,
                Email = customerUpdated.Email,
                AppImageId = customerUpdated.AppImageId,
                ThumbAppImageId = customerUpdated.ThumbAppImageId,
                Vip = customerUpdated.Vip,
                Credit = customerUpdated.Credit,
                CustomerTypeID = customerUpdated.CustomerTypeID,
                CountryID = customerUpdated.CountryID
            };

            return dal.Update(customer, customerOrig);
        }

        public static List<Customer> GetBySellerID(int sellerID, int status, string query, int page)
        {
            int skip = (Convert.ToInt32(page) - 1) * 10;
            return CustomerDAL.GetBySellerID(sellerID, status, query, skip);
        }

        public static List<Customer> ListAll()
        {
            return CustomerDAL.ListAll();
        }

        public static List<Customer> ListNameQuery(string query)
        {
            return CustomerDAL.ListNameQuery(query);
        }

        public static List<Customer> GetBySupplierID(int supplierID, string query, int page)
        {
            int skip = (Convert.ToInt32(page) - 1) * 20;
            return CustomerDAL.GetBySupplierID(supplierID, query, skip);
        }

        public static List<Customer> ListForSyncCDS()
        {
            return CustomerDAL.ListForSyncCDS();
        }

    }
}
