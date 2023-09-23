using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string token { get; set; }
        [Required]
        public string userid { get; set; }
        [Required]
        public string TwoFactorCode { get; set; }
    }
}
