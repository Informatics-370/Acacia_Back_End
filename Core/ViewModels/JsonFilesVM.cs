using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class JsonFilesVM
    {
        [Required]
        public IFormFile ProductList { get; set; }
    }
}
