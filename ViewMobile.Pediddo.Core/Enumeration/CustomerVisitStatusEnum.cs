using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Enumeration
{
    public class CustomerVisitStatusEnum
    {
        public enum Action
        {
            //Cuando solamente marca un cliente para luego ir a visitar.
            CreatePotentialCustomer = 1,
            //----------------------------------------------------------

            //Cuando registra una visita que no genera ningun pedido o muestra de producto.
            CreateVisit = 2,
            //-----------------------------------------------------------------------------

            //Cuando el cliente solicita que se le envie una muestra del producto.
            CreateSampleOrder = 3,
            //--------------------------------------------------------------------

            //Cuando el cliente hace un pedido de producto.
            CreateOrder = 4
            //---------------------------------------------
        }
    }
}
