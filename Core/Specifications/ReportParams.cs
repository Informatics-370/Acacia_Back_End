namespace Acacia_Back_End.Core.Specifications
{
    public class ReportParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string sort { get; set; }
        public int? CategoryId { get; set; }
    }
}
