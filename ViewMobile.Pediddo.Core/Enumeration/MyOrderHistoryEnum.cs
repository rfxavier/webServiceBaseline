using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewMobile.Pediddo.Core.Enumeration
{
    public class MyOrderHistoryEnum
    {
        public enum Status
        {
            AwaitingAttention,
            Processing,
            PendingApproval,
            Approved,
            Executed,
            Rejected,
            Canceled,
            GoingToPoint,
            Delivered,
            NotAccepted,
            Nobody,
            Other
        }

        public enum StatusDisplay
        {
            Iniciado,
            Procesando,
            Pendiente_de_Aprobacion,
            Aprobado,
            Ejecutado,
            Rechazado,
            Cancelado,
            En_Camino,
            Entregado,
            No_aceptado,
            No_Habia_Nadie,
            Otro
        }

        public enum PayMode
        {
            CashPayment = 1,
            CreditPayment = 2
        }

        public enum PaymentMethod
        {
            Cash = 1,
            DebitCard = 2,
            PediddoCredit = 3,
            CreditCard = 4
        }

        public enum InteractionID
        {
            /// <summary>
            /// Generation of new order
            /// </summary>
            NewOrder = 1,
            /// <summary>
            /// Assignment to dealer
            /// </summary>
            Assignment = 2,
            /// <summary>
            /// Going to deliver
            /// </summary>
            GoingToDeliver = 3,
            /// <summary>
            /// Doing the deliver
            /// </summary>
            Delivery = 4,
            /// <summary>
            /// Make the invoice
            /// </summary>
            Invoicing = 5,
            /// <summary>
            /// Make the cancellation
            /// </summary>
            Cancellation = 6

        }

        public static string InteractionIDDescription(int interactionID)
        {
            string[] array = { "Pedido novo", "Direcionado ao representante", "Encaminhado para entrega", "Executando entrega", "Emitindo nota fiscal", "Cancelado" };

            var returnStr = interactionID <= 0 ? string.Empty : array[interactionID - 1];

            return returnStr;
        }

        public static string InteractionIDColor(int interactionID)

        {
            string[] array = { "#b276b2", "#b2912f", "#faa43a", "#60bd68", "#decf3f", "#5da5da", "#f17cb0", "#f15854", "#adff2f", "#e0ffff", "#f0e68c", "#008b8b", "#00ced1", "#2f4f4f", "#006400", "#00008b", "#8b0000" };

            var returnStr = array[interactionID];

            return returnStr;
        }

    }
}
