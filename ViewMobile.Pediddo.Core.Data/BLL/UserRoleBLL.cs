using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class UserRoleBLL
    {
        public UserRole Save(UserRole userRole)
        {
            return new UserRoleDAL().Save(userRole);
        }
        public void Delete(int sellerID)
        {
            new UserRoleDAL().Delete(sellerID);
        }

    }
}