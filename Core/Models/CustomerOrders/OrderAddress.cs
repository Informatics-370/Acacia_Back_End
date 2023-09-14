namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public class OrderAddress
    {
        public OrderAddress()
        {
        }

        public OrderAddress(string firstName, string lastName, string streetAddress, string complexName, string suburb, string city, string province, string postalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            StreetAddress = streetAddress;
            ComplexName = complexName;
            Suburb = suburb;
            City = city;
            Province = province;
            PostalCode = postalCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string ComplexName { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string? TrackingNumber { get; set; }
    }
}
