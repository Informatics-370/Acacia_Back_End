using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.Models.Identities
{
    public class User_Address
    {
        public int Id { get; set; }
        public string StreetAddress { get; set; }
        public string ComplexName { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}