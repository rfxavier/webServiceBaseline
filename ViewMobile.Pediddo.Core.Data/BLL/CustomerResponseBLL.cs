using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CustomerResponseBLL
    {
        public static CustomerResponse GetById(int id)
        {
            return new CustomerResponseDAL().GetById(id);
        }

        public static List<CustomerResponse> List()
        {
            return CustomerResponseDAL.List();
        }
    }
}
