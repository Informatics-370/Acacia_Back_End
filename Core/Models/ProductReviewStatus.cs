using System.Runtime.Serialization;

namespace Acacia_Back_End.Core.Models
{
    public enum ProductReviewStatus
    {
        [EnumMember(Value = "Flagged")]
        Flagged,

        [EnumMember(Value = "Available")]
        Available,
    }
}
