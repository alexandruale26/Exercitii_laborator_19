
namespace Data.Exceptions
{
    public class StudentDoesNotExistsException : Exception
    {
        private const string message = "Student does not exists";

        public StudentDoesNotExistsException() : base(message) 
        {
        }
    }
}