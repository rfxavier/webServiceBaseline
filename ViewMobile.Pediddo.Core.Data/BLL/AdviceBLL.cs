using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class AdviceBLL
    {
        public Advice Save(Advice advice)
        {
            return new AdviceDAL().Save(advice);
        }

        public static Advice GetById(int adviceId)
        {
            return AdviceDAL.GetById(adviceId);
        }

        public static List<Advice> ListNotRead(int buyerId)
        {
            return AdviceDAL.ListNotRead(buyerId);
        }

        public static List<Advice> ListAdvice(int buyerId)
        {
            return AdviceDAL.ListAdvice(buyerId);
        }

        public static List<Advice> ListAdvice(int buyerId, int skip)
        {
            return AdviceDAL.ListAdvice(buyerId, skip);
        }

        public static List<Advice> ListAll()
        {
            return AdviceDAL.ListAll();
        }

        public Advice Update (Advice adviceUpdated)
        {
            AdviceDAL dal = new AdviceDAL();
            Advice adviceOrig = AdviceDAL.GetById(adviceUpdated.AdviceId);
            Advice advice = new Advice()
            {
                Active = adviceUpdated.Active,
                AdviceId = adviceUpdated.AdviceId,
                AppImageId = adviceUpdated.AppImageId,
                Body = adviceUpdated.Body,
                BuyerID = adviceUpdated.BuyerID,
                Call = adviceUpdated.Call,
                Credit = adviceUpdated.Credit,
                DateCreated = adviceUpdated.DateCreated,
                Highlight = adviceUpdated.Highlight,
                IsPublic = adviceUpdated.IsPublic,
                PictureName = adviceUpdated.PictureName,
                Argument = adviceUpdated.Argument,
                Title = adviceUpdated.Title,
                Vip = adviceUpdated.Vip
            };

            return dal.Update(advice, adviceOrig);
        }
    }
}
