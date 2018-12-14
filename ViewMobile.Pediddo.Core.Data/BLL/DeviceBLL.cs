using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class DeviceBLL
    {
        public Device Save(Device device)
        {
            return new DeviceDAL().Save(device);
        }

        public static Device GetByDeviceToken(string deviceToken)
        {
            return DeviceDAL.GetByDeviceToken(deviceToken);
        }

        public static List<Device> ListByBuyerID(int buyerID)
        {
            return DeviceDAL.ListByBuyerID(buyerID);
        }

        public Device Update(Device deviceUpdated)
        {
            DeviceDAL dal = new DeviceDAL();
            Device DeviceOrig = dal.GetById(deviceUpdated.DeviceID);
            Device Device = new Device()
            {
                DeviceID = deviceUpdated.DeviceID,
                Description = deviceUpdated.Description,
                DeviceToken = deviceUpdated.DeviceToken,
                PushToken = deviceUpdated.PushToken,
                SimCardNumber = deviceUpdated.SimCardNumber
            };

            return dal.Update(Device, DeviceOrig);
        }

    }
}
