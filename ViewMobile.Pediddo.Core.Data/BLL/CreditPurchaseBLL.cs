using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CreditPurchaseBLL 
    {
        public CreditPurchase Save(CreditPurchase creditPurchase)
        {
            return new CreditPurchaseDAL().Save(creditPurchase);
        }

        public static List<CreditPurchase> ListByStatus(int status)
        {
            return CreditPurchaseDAL.ListByStatus(status);
        }

        public CreditPurchase Update(CreditPurchase creditPurchaseUpdated)
        {
            CreditPurchaseDAL dal = new CreditPurchaseDAL();
            CreditPurchase creditPurchaseOrig = dal.GetById(creditPurchaseUpdated.CreditPurchaseId);
            CreditPurchase creditPurchase = new CreditPurchase()
            {
                Active = creditPurchaseUpdated.Active,
                Amount = creditPurchaseUpdated.Amount,
                CreditPurchaseId = creditPurchaseUpdated.CreditPurchaseId,
                CustomerID = creditPurchaseUpdated.CustomerID,
                DateCreated = creditPurchaseUpdated.DateCreated,
                PaymentMethod = creditPurchaseUpdated.PaymentMethod,
                Status = creditPurchaseUpdated.Status
            };

            return dal.Update(creditPurchase, creditPurchaseOrig);
        }
    }
}
