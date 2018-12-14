using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class BuyerDeviceBLL
    {
        public BuyerDevice Save(BuyerDevice buyerDevice)
        {
            return new BuyerDeviceDAL().Save(buyerDevice);
        }

        public static BuyerDevice GetByBuyerDevice(Buyer buyer, Device device)
        {
            return BuyerDeviceDAL.GetByBuyerDevice(buyer, device);
        }
    }
}
