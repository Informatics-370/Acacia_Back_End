namespace Acacia_Back_End.Core.ViewModels
{
    public class AuditTrailVM
    {
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}
