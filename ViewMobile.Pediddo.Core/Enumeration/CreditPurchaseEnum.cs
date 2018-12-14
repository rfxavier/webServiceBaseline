using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Enumeration
{
    public class CreditPurchaseEnum
    {
        public enum Status
        {
            AwaitingAttention = 1,
            Accepted = 2,
            Rejected = 3,
            Canceled = 4
        }
    }
}
