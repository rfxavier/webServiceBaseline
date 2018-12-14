using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class ContentSharingLogBLL
    {

        public ContentSharingLog Save(ContentSharingLog contentSharingLog)
        {
            return new ContentSharingLogDAL().Save(contentSharingLog);
        }

        public ContentSharingLog GetById(int id)
        {
            return new ContentSharingLogDAL().GetById(id);
        }

        public ContentSharingLog Update(ContentSharingLog contentSharingLogUpdated)
        {
            ContentSharingLogDAL contentSharingLogDAL = new ContentSharingLogDAL();
            ContentSharingLog contentSharingLogOrig = contentSharingLogDAL.GetById(contentSharingLogUpdated.ContentSharingLogId);

            ContentSharingLog contentSharingLog = new ContentSharingLog()
            {
                Active = contentSharingLogUpdated.Active,
                ContactEmail = contentSharingLogUpdated.ContactEmail,
                ContactName = contentSharingLogUpdated.ContactName,
                ContactPhone = contentSharingLogUpdated.ContactPhone,
                ContentSharingId = contentSharingLogUpdated.ContentSharingId,
                ContentSharingLogId = contentSharingLogUpdated.ContentSharingLogId,
                BuyerID = contentSharingLogUpdated.BuyerID,
                DateCreated = contentSharingLogUpdated.DateCreated,
                UsingIOS = contentSharingLogUpdated.UsingIOS
            };

            return contentSharingLogDAL.Update(contentSharingLog, contentSharingLogOrig);
        }
    }
}
