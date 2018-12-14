using System.Collections.Generic;
using System.Linq;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class RoleDAL : DAOBase<Role, int>
    {
        public static List<Role> ListAll()
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Roles.Where(b => b.Active).ToList();
            }
        }
    }
}
