using System.Collections.Generic;
using System.Linq;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class UserRoleDAL : DAOBase<UserRole, int>
    {
        public void Delete(int sellerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                IEnumerable<UserRole> userRoleToDelete = db.UserRoles.Where(ur => ur.SellerId == sellerID);
                db.UserRoles.DeleteAllOnSubmit(userRoleToDelete);
                db.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
            }
        }
    }
}
