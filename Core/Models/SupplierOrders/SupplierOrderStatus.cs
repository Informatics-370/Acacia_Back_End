using System.Runtime.Serialization;

namespace Acacia_Back_End.Core.Models.SupplierOrders
{
        public enum SupplierOrderStatus
        {
            [EnumMember(Value = "Pending")]
            Pending,

            [EnumMember(Value = "Email Sent")]
            EmailSent,

            [EnumMember(Value = "Payment Confirmed")]
            PaymentConfirmed,

            [EnumMember(Value = "Order Recieved")]
            OrderRecieved,

            [EnumMember(Value = "Cancelled")]
            Cancelled,
        }
}
