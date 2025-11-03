
namespace Domain.Entities.OrderModule
{
    public class Address
    {
        public Address(string firstName, string lastName, string country, string city, string street)
        {
            FirstName = firstName;
            LastName = lastName;
            Country = country;
            City = city;
            Street = street;
        }
        public Address()
        {

        }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

    }
}
