using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Address? Address { get; set; }


        public override string ToString()
        {
            return $"{FirstName} {LastName}, Id: {StudentId}, Age: {Age}";
        }
    }
}
