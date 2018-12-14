using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CustomerResponseDAL : DAOBase<CustomerResponse, int>
    {
        public static List<CustomerResponse> List()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CustomerResponses.Where(v => v.Active == true).ToList();
            }
        }
    }
}
