using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class EmployeeBLL
    {
        public static Employee GetById(int employeeId)
        {
            return new EmployeeDAL().GetById(employeeId);
        }

        public Employee Save(Employee employee)
        {
            return new EmployeeDAL().Save(employee);
        }

        public Employee Update(Employee employeeUpdated)
        {
            EmployeeDAL dal = new EmployeeDAL();
            Employee employeeOrig = dal.GetById(employeeUpdated.EmployeeID);

            Employee employee = new Employee()
            {
                EmployeeID = employeeUpdated.EmployeeID,
                SupplierID = employeeUpdated.SupplierID,
                FirstName = employeeUpdated.FirstName,
                LastName = employeeUpdated.LastName,
                Email = employeeUpdated.Email,
                Password = employeeUpdated.Password,
                PhoneNumber = employeeUpdated.PhoneNumber,
                BirthDate = employeeUpdated.BirthDate,
                Gender = employeeUpdated.Gender,
                SerialKey = employeeUpdated.SerialKey,
                DateCreated = employeeUpdated.DateCreated,
                Active = employeeUpdated.Active
            };

            return dal.Update(employee, employeeOrig);
        }

        public Employee GetByEmail(string email) => new EmployeeDAL().GetByEmail(email);
    }
}
