using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class EmployeeAdviceDAL : DAOBase<EmployeeAdvice, int>
    {
        public static EmployeeAdvice GetById(int employeeAdviceId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.EmployeeAdvices.FirstOrDefault(a => a.EmployeeAdviceId == employeeAdviceId && a.Active == true);
            }
        }

        public static List<EmployeeAdvice> ListNotRead(int employeeId, int employeeType)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeeAdvice>(a => a.EmployeeAdviceButtons);
                db.LoadOptions = dlo;

                return db.EmployeeAdvices.Where(a => a.EmployeeType == employeeType &&
                                             (a.EmployeeID == employeeId || a.EmployeeID == null) &&
                                             !a.EmployeeAdviceLogs.Any(l => l.EmployeeID == employeeId) &&
                                             a.Active == true).ToList();
            }
        }

        public static List<EmployeeAdvice> ListAdvice(int employeeId, int employeeType)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeeAdvice>(a => a.EmployeeAdviceButtons);
                db.LoadOptions = dlo;

                return db.EmployeeAdvices.Where(a => a.EmployeeType == employeeType &&
                                             (a.EmployeeID == employeeId || a.EmployeeID == null) &&
                                             a.Active == true).OrderByDescending(a => a.DateCreated).ToList();
            }
        }

        public static List<EmployeeAdvice> ListAdvice(int employeeId, int employeeType, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeeAdvice>(a => a.EmployeeAdviceButtons);
                db.LoadOptions = dlo;

                return db.EmployeeAdvices.Where(a => a.EmployeeType == employeeType &&
                                             (a.EmployeeID == employeeId || a.EmployeeID == null) &&
                                             a.Active == true).OrderByDescending(a => a.DateCreated).Skip(pSkip).Take(10).ToList();
            }
        }

        public static List<EmployeeAdvice> ListAll()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<EmployeeAdvice>(a => a.EmployeeAdviceButtons);
                db.LoadOptions = dlo;

                return db.EmployeeAdvices.Where(a => a.Active == true).ToList();
            }
        }
    }
}
