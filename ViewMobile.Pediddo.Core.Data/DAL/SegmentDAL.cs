using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SegmentDAL : DAOBase<Segment, int>
    {
        public List<Segment> GetAllByPage(int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Segments.Where(l => l.Active == true).Skip(pSkip).Take(10).ToList();
            }
        }
    }
}
