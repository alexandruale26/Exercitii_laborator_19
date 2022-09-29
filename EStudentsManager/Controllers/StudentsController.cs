using Microsoft.AspNetCore.Mvc;
using EStudentsManager.DTOs;
using Data.Models;
using Data;
using System.Runtime.InteropServices;

namespace EStudentsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        /// <summary>
        /// Return all students
        /// </summary>
        /// <param name="getAddress">If want to get student's address</param>
        /// <returns></returns>
        [HttpGet("all")]
        public IEnumerable<StudentToGet> GetAllStudentsFromDB([Optional][FromQuery] bool getAddress)
        {
            var students = DataLayer.GetAllStudents(getAddress);
            var result = new List<StudentToGet>();

            students.ForEach(s =>
            {
                if(s.Address != null)
                {
                result.Add(new StudentToGet
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Age = s.Age,
                    City = s.Address.City,
                    Street = s.Address.Street,
                    StreetNumber = s.Address.StreetNumber
                });
                }
                else
                {
                    result.Add(new StudentToGet
                    {
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Age = s.Age,
                    });
                }
            });
            return result;
        }

        /// <summary>
        /// Return a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="getAddress">If want to get the student's address</param>
        /// <returns></returns>
        [HttpGet("student/{id}")]
        public StudentToGet GetStudent([FromRoute] int id, [Optional][FromQuery] bool getAddress)
        {
            var student = DataLayer.GetStudent(id, getAddress);

            if (student.Address != null)
            {
                return FullStudentDetails(student.FirstName, student.LastName, student.Age, student.Address.City, student.Address.Street, student.Address.StreetNumber);
            }
            return StudentNoAddressDetails(student.FirstName, student.LastName, student.Age);
        }

        /// <summary>
        /// Create a student
        /// </summary>
        /// <param name="newStudent">Student data</param>
        /// <param name="hasAddress">If new student has address</param>
        /// <returns></returns>
        [HttpPost("create")]
        public StudentToGet CreateStudent([FromBody] StudentToCreate newStudent, [FromQuery] bool hasAddress)
        {
            Student student;

            if (hasAddress)
            {
                student =  DataLayer.CreateStudentWithAddress(newStudent.FirstName, newStudent.LastName, newStudent.Age, newStudent.City, newStudent.Street, newStudent.StreetNumber);
                return FullStudentDetails(student.FirstName, student.LastName, student.Age, student.Address.City, student.Address.Street, student.Address.StreetNumber);

            }
            student =  DataLayer.CreateStudentWithoutAddress(newStudent.FirstName, newStudent.LastName, newStudent.Age);
            return StudentNoAddressDetails(student.FirstName, student.LastName, student.Age);
        }

        /// <summary>
        /// Remove a student
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="removeAddress">If want to remove address from database if address has no students</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public int RemoveStudent([FromRoute]int id, [FromQuery] bool removeAddress)
        {
            return DataLayer.RemoveStudent(id, removeAddress);
        }

        /// <summary>
        /// Update student's data
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <param name="studUpdates">Student's new data</param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public StudentToUpdate ModifyStudent([FromRoute] int id, [FromBody] StudentToUpdate studUpdates)
        {
            DataLayer.ModifyStudentData(id, studUpdates.FirstName, studUpdates.LastName, studUpdates.Age);
            return studUpdates;
        }


        private StudentToGet FullStudentDetails(string firstName, string lastName, int age, string city, string street, int streetNumber)
        {
            return new StudentToGet
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                City = city,
                Street = street,
                StreetNumber = streetNumber
            };
        }

        private StudentToGet StudentNoAddressDetails(string firstName, string lastName, int age)
        {
            return new StudentToGet
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };
        }
    }
}
