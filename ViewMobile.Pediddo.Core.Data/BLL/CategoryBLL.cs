using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class CategoryBLL
    {
        public static Category GetById(int id)
        {
            return new CategoryDAL().GetById(id);
        }


        public static Category GetByName(string name)
        {
            return  CategoryDAL.GetByName(name);
        }

        public Category Save(Category category)
        {
            return new CategoryDAL().Save(category);
        }

        public static List<Category> ListBySupplierID(int pSupplierID)
        {
            return CategoryDAL.ListBySupplierID(pSupplierID);
        }

        public static List<Category> ListByCustomerTypeId(int pSupplierID, int customerTypeID)
        {
            return CategoryDAL.ListByCustomerTypeId(pSupplierID, customerTypeID);
        }

        public static List<Category> ListChildCategory(int categoryId)
        {
            return CategoryDAL.ListChildCategory(categoryId);
        }
    }
}
