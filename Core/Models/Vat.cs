using NPOI.SS.Formula.Functions;

namespace Acacia_Back_End.Core.Models
{
    public class Vat: BaseEntity
    {
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;}
        public bool IsActive { get; set; }
    }
}
