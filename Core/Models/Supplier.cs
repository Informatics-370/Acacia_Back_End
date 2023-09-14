namespace Acacia_Back_End.Core.Models
{
    public class Supplier: BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
    }
}
