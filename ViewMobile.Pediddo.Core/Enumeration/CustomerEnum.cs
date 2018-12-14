using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Enumeration
{
    public class CustomerEnum
    {
        public enum Status
        {
            Opportunity = 1,
            Potential = 2,
            Activo = 3,
            Inactivo = 4
        }

        public enum CustomerTypeID
        {
            Final = 1,
            Distributor = 2
        }
    }
}
