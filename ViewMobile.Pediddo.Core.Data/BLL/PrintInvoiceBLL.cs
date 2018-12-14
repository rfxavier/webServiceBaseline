using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.BLL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class PrintInvoiceBLL
    {
        public PrintInvoice Save(PrintInvoice printInvoice)
        {
            return new PrintInvoiceDAL().Save(printInvoice);
        }

        public static List<PrintInvoice> ListPendingToPrint()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.PrintInvoices.Where(p => !p.Printed).ToList();
            }
        }

        public PrintInvoice Update(PrintInvoice printInvoiceUpdated)
        {
            PrintInvoiceDAL dal = new PrintInvoiceDAL();
            PrintInvoice printInvoiceOrig = dal.GetById(printInvoiceUpdated.PrintInvoiceID);
            PrintInvoice printInvoice = new PrintInvoice()
            {
                Active = printInvoiceUpdated.Active,
                DateCreated = printInvoiceUpdated.DateCreated,
                InvoiceDate = printInvoiceUpdated.InvoiceDate,
                InvoiceNumber = printInvoiceUpdated.InvoiceNumber,
                MyOrderID = printInvoiceUpdated.MyOrderID,
                Printed = printInvoiceUpdated.Printed,
                PrintInvoiceID = printInvoiceUpdated.PrintInvoiceID,
                ResultMessage = printInvoiceUpdated.ResultMessage
            };

            return dal.Update(printInvoice, printInvoiceOrig);
        }
    }
}
