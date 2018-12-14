using System.Collections.Generic;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class RoleBLL
    {
        public static List<Role> ListAll()
        {
            return RoleDAL.ListAll();
        }
    }
}
