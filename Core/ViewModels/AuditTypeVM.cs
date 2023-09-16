using System.Runtime.Serialization;

namespace Acacia_Back_End.Core.ViewModels
{
    public enum AuditTypeVM
    {
        [EnumMember(Value = "Sale Order")]
        SaleOrder,

        [EnumMember(Value = "Supplier Order")]
        SupplierOrder,

        [EnumMember(Value = "Sale Return")]
        SaleReturn,

        [EnumMember(Value = "Supplier Return")]
        SupplierReturn,
    }
}
