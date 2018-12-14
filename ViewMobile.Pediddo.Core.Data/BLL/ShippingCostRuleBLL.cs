using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViewMobile.Pediddo.Core.Data.DAO;
using ViewMobile.Pediddo.Core.Data.DAL;

namespace ViewMobile.Pediddo.Core.Data.BLL
{
    public class ShippingCostRuleBLL
    {
        public static ShippingCostRule GetById(int id)
        {
            return new ShippingCostRuleDAL().GetById(id);
        }

        public static List<ShippingCostRule> GetAll()
        {
            return new ShippingCostRuleDAL().GetAll();
        }

        public ShippingCostRule Update(ShippingCostRule shippingCostRuleUpd)
        {
            ShippingCostRuleDAL dal = new ShippingCostRuleDAL();
            ShippingCostRule shippingCostRuleOrig = dal.GetById(shippingCostRuleUpd.ShippingCostRuleId);
            ShippingCostRule shippingCostRule = new ShippingCostRule()
            {
                Active = shippingCostRuleUpd.Active,
                ShippingCostRuleId = shippingCostRuleUpd.ShippingCostRuleId,
                Value = shippingCostRuleUpd.Value
            };

            return dal.Update(shippingCostRule, shippingCostRuleOrig);
        }
    }
}
