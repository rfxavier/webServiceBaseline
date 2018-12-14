using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class ProductBLL
    {
        public static Product GetById(int productId)
        {
            return new ProductDAL().GetById(productId);
        }

        public void Save(Product product)
        {
            new ProductDAL().Save(product);
        }

       
        public void BulkSave(List<Product> productList)
        {
            new ProductDAL().BulkSave(productList);
        }

        public static List<Product> ListBySupplierID(int pSupplierID)
        {
            return ProductDAL.ListBySupplierID(pSupplierID);
        }
        public static List<Product> ListBySupplierIDByPage(int pSupplierID, int pSkip)
        {
            return ProductDAL.ListBySupplierIDByPage(pSupplierID, pSkip);
        }
        public static List<Product> GetAll()
        {
            return new ProductDAL().GetAll();
        }

        public static List<Product> ListByCategoryID(int pCategoryID)
        {
            return ProductDAL.ListByCategoryID(pCategoryID);
        }

        public static List<Product> ListByCategoryTempID(int pCategoryID, int customerTypeId)
        {
            return ProductDAL.ListByCategoryTempID(pCategoryID, customerTypeId);
        }

        public static List<Product> ListByCategoryIDByPage(int pCategoryID, int pSkip)
        {
            return ProductDAL.ListByCategoryIDByPage(pCategoryID, pSkip);
        }

        public Product Update(Product productUpdated)
        {
            ProductDAL dal = new ProductDAL();
            Product productOrig = dal.GetById(productUpdated.ProductID);
            Product product = new Product()
            {
                Active = productUpdated.Active,
                DateCreated = productUpdated.DateCreated,
                ExternalID = productUpdated.ExternalID,
                CategoryID = productUpdated.CategoryID,
                CategoryTempID = productUpdated.CategoryID,
                Discount = productUpdated.Discount,
                CostShow = productUpdated.CostShow,
                Cost = productUpdated.Cost,
                ThumbAppImageID = productUpdated.ThumbAppImageID,
                AppImageID = productUpdated.AppImageID,
                Description = productUpdated.Description,
                Name = productUpdated.Name,
                ProductID = productUpdated.ProductID,
                Manufacturer = productUpdated.Manufacturer,
                Udm = productUpdated.Udm,
                IvaType = productUpdated.IvaType
            };

            return dal.Update(product, productOrig);
        }

        public static List<Product> ListInOffer(int pSupplierID, int customerTypeId)
        {
            return ProductDAL.ListInOffer(pSupplierID, customerTypeId);
        }
    }
}
