namespace Acacia_Back_End.Core.ViewModels
{
    public class ResetPasswordVM
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string token { get; set; }
        public string userid { get; set; }
        public string TwoFactorCode { get; set; }
    }
}
