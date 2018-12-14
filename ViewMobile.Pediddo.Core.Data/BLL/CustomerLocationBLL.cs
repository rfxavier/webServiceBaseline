using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CustomerLocationBLL
    {
        public CustomerLocation Save(CustomerLocation customerLocation)
        {
            return new CustomerLocationDAL().Save(customerLocation);
        }

        public CustomerLocation GetById(int customerLocationId)
        {
            return new CustomerLocationDAL().GetById(customerLocationId);
        }

        public static List<CustomerLocation> ListByCustomerID(int customerID, bool temporary)
        {
            return CustomerLocationDAL.ListByCustomerID(customerID, temporary);
        }

        public CustomerLocation Update(CustomerLocation customerLocationUpdated)
        {
            CustomerLocationDAL dal = new CustomerLocationDAL();
            CustomerLocation customerLocationOrig = dal.GetById(customerLocationUpdated.CustomerLocationID);
            CustomerLocation customerLocation = new CustomerLocation()
            {
                Active = customerLocationUpdated.Active,
                Description = customerLocationUpdated.Description,
                Address = customerLocationUpdated.Address,
                AppImageID = customerLocationUpdated.AppImageID,
                CityID = customerLocationUpdated.CityID,
                Complement = customerLocationUpdated.Complement,
                DateCreated = customerLocationUpdated.DateCreated,
                Latitude = customerLocationUpdated.Latitude,
                Locality = customerLocationUpdated.Locality,
                CustomerLocationID = customerLocationUpdated.CustomerLocationID,
                CustomerID = customerLocationUpdated.CustomerID,
                Longitude = customerLocationUpdated.Longitude,
                Number = customerLocationUpdated.Number,
                Reference = customerLocationUpdated.Reference,
                Temporary = customerLocationUpdated.Temporary
            };

            return dal.Update(customerLocation, customerLocationOrig);
        }
    }
}
    