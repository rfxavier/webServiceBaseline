using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class BuyerDeviceDAL : DAOBase<BuyerDevice, int>
    {
        public static BuyerDevice GetByBuyerDevice(Buyer pBuyer, Device pDevice)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.BuyerDevices.FirstOrDefault(bd => bd.BuyerID == pBuyer.BuyerID && bd.DeviceID == pDevice.DeviceID);
            }
        }
    }
}
