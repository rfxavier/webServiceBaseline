using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class EmployeeDeviceBLL
    {
        public EmployeeDevice Save(EmployeeDevice employeeDevice)
        {
            return new EmployeeDeviceDAL().Save(employeeDevice);
        }

        public static EmployeeDevice GetByEmployeeDevice(Employee employee, Device device)
        {
            return EmployeeDeviceDAL.GetByEmployeeDevice(employee, device);
        }
    }
}
