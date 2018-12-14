using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class EmployeeDeviceDAL : DAOBase<EmployeeDevice, int>
    {
        public static EmployeeDevice GetByEmployeeDevice(Employee employee, Device device)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.EmployeeDevices.FirstOrDefault(sd => sd.EmployeeID == employee.EmployeeID && sd.DeviceID == device.DeviceID);
            }
        }
    }
}
