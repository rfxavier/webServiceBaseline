using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

using ViewMobile.Pediddo.Core.Enumeration;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class CategoryDAL : DAOBase<Category, int>
    {
        public static List<Category> ListBySupplierID(int supplierID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //db.DeferredLoadingEnabled = false;
                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Category>(c => c.Categories);
                //db.LoadOptions = dlo;

                return db.Categories.Where(p => p.SupplierID == supplierID && p.Active == true).ToList();
            }
        }


        
        
        public static Category GetByName(string name)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                db.DeferredLoadingEnabled = false;
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Category>(c => c.CustomerTypeCategories);
                dlo.LoadWith<CustomerTypeCategory>(c => c.CustomerType);
                db.LoadOptions = dlo;
                return db.Categories.Where(c => c.Name == name).FirstOrDefault();
            }
        }


        public static List<Category> ListByCustomerTypeId(int pSupplierID, int customerTypeID)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //db.DeferredLoadingEnabled = false;
                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Category>(c => c.Categories);
                //db.LoadOptions = dlo;

                return db.Categories.Where(c => c.SupplierID == pSupplierID &&
                                                c.CustomerTypeCategories.Any(ctc => ctc.CustomerTypeID == customerTypeID) &&
                                                c.Active.Value
                                                ).ToList();
            }
        }

        public static List<Category> ListChildCategory(int categoryId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                //db.DeferredLoadingEnabled = false;
                //DataLoadOptions dlo = new DataLoadOptions();
                //dlo.LoadWith<Category>(c => c.Categories);
                //db.LoadOptions = dlo;

                return db.Categories.Where(c => c.ParentID == categoryId &&
                                                c.Active.Value
                                                ).ToList();
            }
        }
    }
}
