using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Enumeration;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CustomerTypeDAL : DAOBase<Customer, int>
    {

        public static CustomerType GetByName(string name)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CustomerTypes.Where(c => c.Name.ToUpper().Contains(name.ToUpper()) && c.Active == true).FirstOrDefault();
            }
        }


    }
}
