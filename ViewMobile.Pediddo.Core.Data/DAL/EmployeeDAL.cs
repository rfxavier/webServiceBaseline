using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class EmployeeDAL : DAOBase<Employee, int>
    {
        public Employee GetByEmail(string email)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                var query = from em in db.Employees where em.Email.Equals(email) select em;

                return query.FirstOrDefault<Employee>();
            }
        }
    }
}
