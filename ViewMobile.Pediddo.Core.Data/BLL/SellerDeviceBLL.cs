using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SellerDeviceBLL
    {
        public SellerDevice Save(SellerDevice buyerDevice)
        {
            return new SellerDeviceDAL().Save(buyerDevice);
        }

        public static SellerDevice GetBySellerDevice(Seller seller, Device device)
        {
            return SellerDeviceDAL.GetBySellerDevice(seller, device);
        }
    }
}
