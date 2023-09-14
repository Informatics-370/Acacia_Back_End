using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class CustomerReturnVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyList<ReturnItemVM> ReturnItems { get; set; }
    }
}
