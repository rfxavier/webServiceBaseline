using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class MeterBLL
    {
        public static Meter GetById(int meterId)
        {
            return new MeterDAL().GetById(meterId);
        }

        public static Meter GetByName(string meterName)
        {
            return MeterDAL.GetByName(meterName);
        }
    }
}
