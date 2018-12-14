using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class AppImageBLL
    {
        public static AppImage GetById(int id)
        {
            return new AppImageDAL().GetById(id);
        }

        public AppImage Save(AppImage appImage)
        {
            return new AppImageDAL().Save(appImage);
        }

        public AppImage Update(AppImage appImageUpdated)
        {
            AppImageDAL dal = new AppImageDAL();
            AppImage appImageOrig = dal.GetById(appImageUpdated.AppImageID);
            AppImage appImage = new AppImage()
            {
                Active = true,
                AppImageID = appImageUpdated.AppImageID,
                DateCreated = appImageUpdated.DateCreated,
                ImageName = appImageUpdated.ImageName
            };

            return dal.Update(appImage, appImageOrig);
        }
    }
}
