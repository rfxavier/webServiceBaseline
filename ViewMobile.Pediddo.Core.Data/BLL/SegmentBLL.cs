using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SegmentBLL
    {
        public static List<Segment> GetAll()
        {
            return new SegmentDAL().GetAll();
        }

        public static List<Segment> GetAllByPage(int skip)
        {
            return new SegmentDAL().GetAllByPage(skip);
        }
    }
}
