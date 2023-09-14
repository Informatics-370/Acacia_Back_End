using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class User_AddressVM
    {
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string ComplexName { get; set; }
        [Required]
        public string Suburb { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string PostalCode { get; set; }
    }
}
