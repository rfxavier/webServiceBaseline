using System;
using System.Collections.Generic;
using System.Linq;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class SellerBLL
    {
        public Seller Save(Seller seller)
        {
            return new SellerDAL().Save(seller);
        }

        public Seller Update(Seller sellerUpdated)
        {
            SellerDAL dal = new SellerDAL();
            Seller sellerOrig = dal.GetById(sellerUpdated.SellerID);

            Seller seller = new Seller()
            {
                SellerID = sellerUpdated.SellerID,
                IsBlocked = sellerUpdated.IsBlocked,
                IsAdmin = sellerUpdated.IsAdmin,
                DateCreated = sellerUpdated.DateCreated,
                Active = sellerUpdated.Active
            };

            return dal.Update(seller, sellerOrig);
        }

        public static Seller GetById(int sellerId)
        {
            return new SellerDAL().GetById(sellerId);
        }

        public Seller Authentication(string email, string pPassword)
        {
            return SellerDAL.Authentication(email, pPassword);
        }

        public static List<Seller> ListBySupplierID(int supplierID)
        {
            return SellerDAL.ListBySupplierID(supplierID);
        }

        public static Seller GetByEmail(string email)
        {
            return SellerDAL.GetByEmail(email);
        }

        /// <summary>
        /// Atualiza o Password de SecurityUser desde que encontre o SecurityUser no banco pesquisando 
        /// pelo email e Password atual do Userdmin
        /// </summary>
        /// <param name="SecurityUser">Utilizado para filtrar o SecurityUser utilizando a propriedade email</param>
        /// <param name="actualPassword">Utilizado para filtrar o SecurityUser pelo Password</param>
        /// <param name="newPassword">Novo Password a ser gravado no objeto SecurityUser</param>
        /// <returns>Retorna um objeto do tipo SecurityUser com o novo Password</returns>
        public static Seller ChangePassword(Seller seller, string actualPassword, string newPassword)
        {
            using (DataClasses1DataContext repositorio = new DataClasses1DataContext())
            {
                ///Decodifica o Password atual
                string actualPasswordEncript = new Security.Security().MD5(actualPassword);
                ///Decodifica o novo Password
                string newPasswordEncript = new Security.Security().MD5(newPassword);

                Seller _Seller = repositorio.Sellers.SingleOrDefault(
                                  has => has.Employee.Email == seller.Employee.Email && has.Employee.Password == actualPasswordEncript);

                ///Caso encontre o SecurityUser no banco de dados faz a gravação do novo Password
                if (_Seller != null)
                {
                    _Seller.Employee.Password = newPasswordEncript;
                    repositorio.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    return _Seller;
                }

                return _Seller;
            }
        }

        public static Seller GetBySerialKey(string serialKey)
        {
            return SellerDAL.GetBySerialKey(serialKey);
        }

        public static List<spLastSellerLocationResult> ListLastSellerLocation(DateTime fecha)
        {
            return SellerDAL.ListLastSellerLocation(fecha);
        }

        public static List<spLastSellerAliveResult> ListLastSellerAlive(DateTime fecha)
        {
            return SellerDAL.ListLastSellerAlive(fecha);
        }

        public static List<Seller> ListAdmin(int supplierID)
        {
            return SellerDAL.ListAdmin(supplierID);
        }

        public static List<Seller> ListAll(int supplierID)
        {
            return SellerDAL.ListAll(supplierID);
        }
    }
}
