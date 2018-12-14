using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class DeviceDAL : DAOBase<Device, int>
    {
        public static Device GetByDeviceToken(string deviceToken)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Devices.FirstOrDefault(a => a.DeviceToken == deviceToken);
            }
        }

        public static List<Device> ListByBuyerID(int pBuyerID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Device>(p => p.BuyerDevices);
                db.LoadOptions = dlo;

                return db.Devices.Where(a => a.BuyerDevices.Any(bd => bd.BuyerID == pBuyerID) && a.Active == true).ToList();
            }
        }
    }
}
