using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class UnitMeasureDAL : DAOBase<Category, int>
    {

        public static UnitMeasure GetBySymbol(string symbol)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.UnitMeasures.Where(c => c.Symbol == symbol).FirstOrDefault();
            }
        }

    }
}
