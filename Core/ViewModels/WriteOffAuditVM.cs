namespace Acacia_Back_End.Core.ViewModels
{
    public class WriteOffAuditVM
    {
        public string Email { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
    }
}
