using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class ContentSharingBLL
    {
        public ContentSharing Save(ContentSharing content)
        {
            return new ContentSharingDAL().Save(content);
        }

        public static ContentSharing GetByCode(string code)
        {
            return ContentSharingDAL.GetByCode(code);
        }
    }
}
