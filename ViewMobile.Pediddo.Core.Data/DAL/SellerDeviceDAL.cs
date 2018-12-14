using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class SellerDeviceDAL : DAOBase<SellerDevice, int>
    {
        public static SellerDevice GetBySellerDevice(Seller seller, Device device)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.SellerDevices.FirstOrDefault(sd => sd.SellerID == seller.SellerID && sd.DeviceID == device.DeviceID);
            }
        }
    }
}
