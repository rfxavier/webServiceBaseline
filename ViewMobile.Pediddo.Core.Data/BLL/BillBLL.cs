using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class BillBLL
    {
        public Bill Save(Bill customer)
        {
            return new BillDAL().Save(customer);
        }

        public Bill Update(Bill billUpdated)
        {
            BillDAL dal = new BillDAL();
            Bill billOrig = dal.GetById(billUpdated.BillID);

            Bill bill = new Bill()
            {
                BillID = billUpdated.BillID,
                CustomerID = billUpdated.CustomerID,
                ProductName = billUpdated.ProductName,
                Value = billUpdated.Value,
                Paid = billUpdated.Paid,
                Obs = billUpdated.Obs
            };

            return dal.Update(bill, billOrig);
        }

        public Bill GetById(int billId)
        {
            return new BillDAL().GetById(billId);
        }

    }
}