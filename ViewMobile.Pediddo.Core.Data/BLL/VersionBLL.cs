using System.Collections.Generic;
using ViewMobile.Pediddo.Core.Data.DAL;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class VersionBLL
    {
        public Version GetById(int VersionID)
        {
            return new VersionDAL().GetById(VersionID);
        }

        public static IEnumerable<DAO.Version> GetAll()
        {
            return new VersionDAL().GetAll();
        }

        public DAO.Version Save(Version version)
        {
            return new VersionDAL().Save(version);
        }

        public Version Update(Version versionToUpdate)
        {
            return new VersionDAL().Update(versionToUpdate, null);
        }

        public Version GetMinimumVersion(int OS)
        {
            return new VersionDAL().GetMinimumVersion(OS);
        }
    }

}