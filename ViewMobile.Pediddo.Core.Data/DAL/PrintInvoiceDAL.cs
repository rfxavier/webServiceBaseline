using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class PrintInvoiceDAL : DAOBase<PrintInvoice, int>
    {
        public static List<PrintInvoice> ListPendingToPrint()
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.PrintInvoices.Where(p => !p.Printed).ToList();
            }
        }
    }
}
