using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Runtime.Serialization;

namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment Confirmed")]
        PaymentConfirmed,

        [EnumMember(Value = "Pamyment Failed")]
        PamymentFailed,

        [EnumMember(Value = "Dispatched")]
        Dispatched,

        [EnumMember(Value = "ReadyForCollection")]
        ReadyForCollection,
    }
}
