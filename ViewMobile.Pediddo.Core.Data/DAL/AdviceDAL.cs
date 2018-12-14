using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewMobile.Pediddo.Core.Data.DAO;

namespace ViewMobile.Pediddo.Core.Data.DAL
{
    public class AdviceDAL : DAOBase<Advice, int>
    {
        public static Advice GetById(int adviceId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                return db.Advices.FirstOrDefault(a => a.AdviceId == adviceId && a.Active == true);
            }
        }

        public static List<Advice> ListNotRead(int buyerId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Advice>(a => a.AdviceButtons);
                db.LoadOptions = dlo;

                Buyer buyer = new BuyerDAL().GetById(buyerId);

                return db.Advices.Where(a => (a.BuyerID == buyerId || a.BuyerID == null) &&
                                             (a.Vip == null || (buyer.Customer.Vip ?? false) == a.Vip) &&
                                             (a.Credit == null || (buyer.Customer.Credit ?? 0) >= a.Credit) &&
                                             !a.BuyerAdviceLogs.Any(l => l.BuyerID == buyerId) &&
                                             a.Active == true).ToList();
            }
        }

        public static List<Advice> ListAdvice(int buyerId)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Advice>(a => a.AdviceButtons);
                db.LoadOptions = dlo;

                Buyer buyer = new BuyerDAL().GetById(buyerId);

                return db.Advices.Where(a => (a.BuyerID == buyerId || a.BuyerID == null) &&
                                             (a.Vip == null || (buyer.Customer.Vip ?? false) == a.Vip) &&
                                             (a.Credit == null || (buyer.Customer.Credit ?? 0) >= a.Credit) &&
                                             a.Active == true).OrderByDescending(a => a.DateCreated).ToList();
            }
        }

        public static List<Advice> ListAdvice(int buyerId, int pSkip)
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Advice>(a => a.AdviceButtons);
                db.LoadOptions = dlo;

                Buyer buyer = new BuyerDAL().GetById(buyerId);

                return db.Advices.Where(a => (a.BuyerID == buyerId || a.BuyerID == null) &&
                                             (a.Vip == null || (buyer.Customer.Vip ?? false) == a.Vip) &&
                                             (a.Credit == null || (buyer.Customer.Credit ?? 0) >= a.Credit) &&
                                             a.Active == true).OrderByDescending(a => a.DateCreated).Skip(pSkip).Take(10).ToList();
            }
        }

        public static List<Advice> ListAll()
        {
            using (DataClasses1DataContext db = new DataClasses1DataContext())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Advice>(a => a.AdviceButtons);
                db.LoadOptions = dlo;

                return db.Advices.Where(a => a.Active == true).ToList();
            }
        }
    }
}
