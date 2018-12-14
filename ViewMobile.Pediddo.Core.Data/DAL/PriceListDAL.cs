using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Enumeration;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class PriceListDAL : DAOBase<PriceList, int>
    {

        public PriceList GetByName(string name)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.PriceLists.Where(c => c.Name == name).FirstOrDefault();
            }
        }

        public static List<PriceList> ListAll()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //db.DeferredLoadingEnabled = false;
                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Customer>(p => p.Buyers);
                //dlo.LoadWith<Customer>(p => p.Segment);
                //dlo.LoadWith<Buyer>(p => p.BuyerSellers);
                //dlo.LoadWith<BuyerSeller>(p => p.Seller);
                //dlo.LoadWith<Seller>(p => p.Employee);
                //db.LoadOptions = dlo;

                return db.PriceLists.Where(c => c.Active == true).ToList();
            }
        }



    }
}
