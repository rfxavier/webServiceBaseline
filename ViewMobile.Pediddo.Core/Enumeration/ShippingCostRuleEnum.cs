using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Enumeration
{
    public class ShippingCostRuleEnum
    {
        public enum ShippingCostRuleId
        {
            CustomerWithCredit = 1,
            OrderAmount = 2,
            VipClient = 3
        }
    }
}
