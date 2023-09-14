using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class WriteOffVM
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Product { get; set; }
        public string Reason { get; set; }
    }
}
