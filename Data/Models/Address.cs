using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Data.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }

        [JsonIgnore]
        public List<Student> Students { get; set; } = new List<Student>();


        public override string ToString()
        {
            return $"Address: {City}, {Street} {StreetNumber}";
        }
    }
}