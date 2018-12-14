using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CustomerLocationDAL : DAOBase<CustomerLocation, int>
    {
        public static List<CustomerLocation> ListByCustomerID(int customerID, bool temporary)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CustomerLocations.Where(cl => cl.CustomerID == customerID && 
                                                  (cl.Temporary ?? false) == temporary && 
                                                  cl.Active == true).ToList();
            }
        }

        public CustomerLocation GetById(int customerLocationId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CustomerLocations.FirstOrDefault(cl => cl.CustomerLocationID == customerLocationId);
            }
        }
    }
}
