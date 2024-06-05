
namespace ObjectPathLibrary.Tests
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
    }

    public record Employee(string Name, int Id, Department Department);

    public record Department(string Name, Manager Manager);

    public record Manager(string Name, string Email);
}

