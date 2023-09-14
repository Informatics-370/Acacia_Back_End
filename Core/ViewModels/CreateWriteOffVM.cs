using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class CreateWriteOffVM
    { 

        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
