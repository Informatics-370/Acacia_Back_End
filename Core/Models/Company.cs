namespace Acacia_Back_End.Core.Models
{
    public class Company:BaseEntity
    {
        public int VatNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int PostalCode { get; set; }
    }
}
