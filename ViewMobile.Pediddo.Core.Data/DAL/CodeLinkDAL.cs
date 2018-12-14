using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CodeLinkDAL : DAOBase<CodeLink, int>
    {
        public static CodeLink GetByCode(string pCode)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.CodeLinks.FirstOrDefault(c=> c.Code == pCode && c.Used == false && c.Active == true);
            }
        }

        public void MarkCodeAsNotUsed(string Code)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.ExecuteCommand($"update codelink set Used = 0 where code = '{Code}'");
            }
        }

    }
}
