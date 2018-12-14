using System.Linq;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class VersionDAL: DAOBase<Version,int>
    {
        public Version GetMinimumVersion(int OS)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Versions.FirstOrDefault(v => v.OS == OS && v.Active == true);
            }
        }
    }
}
