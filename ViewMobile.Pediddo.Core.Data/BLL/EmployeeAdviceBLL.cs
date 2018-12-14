using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class EmployeeAdviceBLL
    {
        public EmployeeAdvice Save(EmployeeAdvice advice)
        {
            return new EmployeeAdviceDAL().Save(advice);
        }

        public static EmployeeAdvice GetById(int adviceId)
        {
            return EmployeeAdviceDAL.GetById(adviceId);
        }

        public static List<EmployeeAdvice> ListNotRead(int employeeId, int employeeType)
        {
            return EmployeeAdviceDAL.ListNotRead(employeeId, employeeType);
        }

        public static List<EmployeeAdvice> ListAdvice(int employeeId, int employeeType)
        {
            return EmployeeAdviceDAL.ListAdvice(employeeId, employeeType);
        }

        public static List<EmployeeAdvice> ListAdvice(int employeeId, int employeeType, int skip)
        {
            return EmployeeAdviceDAL.ListAdvice(employeeId, employeeType, skip);
        }

        public static List<EmployeeAdvice> ListAll()
        {
            return EmployeeAdviceDAL.ListAll();
        }

        public EmployeeAdvice Update(EmployeeAdvice adviceUpdated)
        {
            EmployeeAdviceDAL dal = new EmployeeAdviceDAL();
            EmployeeAdvice adviceOrig = EmployeeAdviceDAL.GetById(adviceUpdated.EmployeeAdviceId);
            EmployeeAdvice advice = new EmployeeAdvice()
            {
                Active = adviceUpdated.Active,
                EmployeeAdviceId = adviceUpdated.EmployeeAdviceId,
                AppImageId = adviceUpdated.AppImageId,
                Body = adviceUpdated.Body,
                EmployeeID = adviceUpdated.EmployeeID,
                EmployeeType = adviceUpdated.EmployeeType,
                Call = adviceUpdated.Call,
                DateCreated = adviceUpdated.DateCreated,
                Highlight = adviceUpdated.Highlight,
                IsPublic = adviceUpdated.IsPublic,
                PictureName = adviceUpdated.PictureName,
                Argument = adviceUpdated.Argument,
                Title = adviceUpdated.Title
            };

            return dal.Update(advice, adviceOrig);
        }
    }
}
