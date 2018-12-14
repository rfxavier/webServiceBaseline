using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class PriceListBLL
    {

        public static PriceList GetById(int priceListID)
        {
            return new PriceListDAL().GetById(priceListID);
        }

        public static PriceList GetName(string name)
        {
            return new PriceListDAL().GetByName(name);
        }

        public static List<PriceList> ListAll()
        {
            return PriceListDAL.ListAll();
        }

    }
}
