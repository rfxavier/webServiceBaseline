using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class MeterDAL : DAOBase<Meter, int>
    {
        
        public static Meter GetByName(string meterName)
        {
            using(DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Meter>(mt => mt.MeterCategories);
                dlo.LoadWith<MeterCategory>(mt => mt.MeterCategoryProducts);
                dlo.LoadWith<MeterCategoryProduct>(mt => mt.Product);
                dlo.LoadWith<Product>(mt => mt.PriceListDetails);
                dlo.LoadWith<PriceListDetail>(mt => mt.PriceList);
                db.LoadOptions = dlo;

                return db.Meters.FirstOrDefault(mt => mt.Name.Equals(meterName) &&
                                                    mt.Enable &&
                                                    mt.Active);
            }
        }
    }
}
