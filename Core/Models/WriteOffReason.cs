using System.Runtime.Serialization;


namespace Acacia_Back_End.Core.Models
{
    public enum WriteOffReason
    {
        [EnumMember(Value = "Damaged Stock")]
        Damaged,

        [EnumMember(Value = "Stolen stock")]
        Theft,

        [EnumMember(Value = "Obsolete Stock")]
        Obsolete,

        [EnumMember(Value = "Losted stock")]
        Losted,

        [EnumMember(Value = "Other")]
        Other,
    }
}
