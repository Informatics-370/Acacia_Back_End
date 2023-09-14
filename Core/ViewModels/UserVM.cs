using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class UserVM
    {

        public string Email { get; set; }

        public ICollection<string> Roles { get; set; }

        public string DisplayName { get; set; }

        public string? ProfilePicture { get; set; }

        public string Token { get; set; }
    }
}
