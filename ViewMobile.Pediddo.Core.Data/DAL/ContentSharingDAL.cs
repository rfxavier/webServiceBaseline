using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class ContentSharingDAL : DAOBase<ContentSharing, int>
    {
        public static ContentSharing GetByCode(string code)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<ContentSharing>(cs => cs.AppImage);
                db.LoadOptions = dlo;

                return db.ContentSharings.FirstOrDefault(cs => cs.Code.Equals(code) && cs.Active == true);
            }
        }
    }
}
