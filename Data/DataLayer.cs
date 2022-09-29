using Data.Models;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Data.Exceptions;

namespace Data
{
    public static class DataLayer
    {
        public static List<Student> GetAllStudents(bool returnAddress)
        {
            using var context = new StudentsManagerContextDB();

            if (returnAddress)
            {
                return context.Students.Include(s => s.Address).ToList();
            }
            return context.Students.ToList();
        }

        public static Student GetStudent(int studentId, bool returnAddress)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }

            if (returnAddress)
            {
            return context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);
            }
            return context.Students.First(s => s.StudentId == studentId);
        }

        public static Student CreateStudentWithoutAddress(string firstName, string lastName, int age)
        {
            using var context = new StudentsManagerContextDB();
            Student newStudent = new Student();

            newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
            return newStudent;
        }

        public static Student CreateStudentWithAddress(string firstName, string lastName, int age, string city, string street, int streetNumber)
        {
            using var context = new StudentsManagerContextDB();

            int DBAddressId = GetAddressID(city, street, (int)streetNumber);
            Student newStudent = new Student();
            Address newAddress = new Address();

            if (DBAddressId == 0)
            {
                newAddress = CreateAddress(city, street, (int)streetNumber);
            }
            else
            {
                newAddress = context.Addresses.First(a => a.AddressId == DBAddressId);
            }

            newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age,
            };

            context.Students.Add(newStudent);
            context.SaveChanges();

            newStudent.Address = newAddress;

            context.SaveChanges();
            return newStudent;
        }

        public static int RemoveStudent(int studentId, bool removeAddress)
        {
            using var context = new StudentsManagerContextDB();

            Student existingStudent = GetStudent(studentId, true);
            int studentAddressId = 0;

            if (existingStudent.Address != null)
            {
                studentAddressId = existingStudent.Address.AddressId;
            }

            context.Students.Remove(existingStudent);
            context.SaveChanges();


            if (removeAddress && studentAddressId > 0)
            {
                RemoveAddressIfIsEmpty(studentAddressId);
            }
            return existingStudent.StudentId;
        }

        public static Student ModifyStudentData(int studentId, string? firstName, string? lastName, int? age)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }

            Student studentToModify = context.Students.First(s => s.StudentId == studentId);

            if (firstName != null) studentToModify.FirstName = firstName;

            if (lastName != null) studentToModify.LastName = lastName;

            if (age != null && age > 0) studentToModify.Age = (int)age;

            context.SaveChanges();
            return studentToModify;
        }

        public static Address ModifyStudentAddress(int studentId, bool removeAddressIfEmpty, string? city, string? street, int? streetNumber)
        {
            using var context = new StudentsManagerContextDB();

            if (!context.Students.Any(s => s.StudentId == studentId))
            {
                throw new StudentDoesNotExistsException();
            }

            Student existingStudent = context.Students.Include(s => s.Address).First(s => s.StudentId == studentId);

            if (city == null || street == null || streetNumber == null || streetNumber == 0) return existingStudent.Address;

            Address newAddress = new Address();
            int oldAddressId = existingStudent.Address.AddressId;
            int addressId = GetAddressID(city, street, (int)streetNumber);

            if (addressId != 0)
            {
                newAddress = context.Addresses.First(a => a.AddressId == addressId);
                existingStudent.Address = newAddress;
                context.SaveChanges();
               
                if (removeAddressIfEmpty) RemoveAddressIfIsEmpty(oldAddressId);
                return newAddress;
            }

            newAddress = CreateAddress(city, street, (int)streetNumber);
            existingStudent.Address = newAddress;
            context.SaveChanges();

            if (removeAddressIfEmpty) RemoveAddressIfIsEmpty(oldAddressId);
            return newAddress;
        }


        private static Address CreateAddress(string city, string street, int number)
        {
            using var context = new StudentsManagerContextDB();

            Address newAddress = new Address
            {
                City = city,
                Street = street,
                StreetNumber = number
            };

            context.Add(newAddress);
            context.SaveChanges();
            return newAddress;
        }

        private static int GetAddressID(string city, string street, int number)
        {
            using var context = new StudentsManagerContextDB();

            var existingAddress = context.Addresses
                .Where(a => a.City.ToLower() == city.ToLower())
                .Where(a => a.Street.ToLower() == street.ToLower())
                .FirstOrDefault(a => a.StreetNumber == number);

            if (existingAddress != null)
            {
                return existingAddress.AddressId;
            }
            return 0;
        }

        private static void RemoveAddressIfIsEmpty(int addressId)
        {
            using var context = new StudentsManagerContextDB();

            if (context.Addresses.Any(a => a.AddressId == addressId) && context.Addresses.Include(s => s.Students).First(a => a.AddressId == addressId).Students.Count == 0)
            {
                context.Addresses.Remove(context.Addresses.First(a => a.AddressId == addressId));
            }
            context.SaveChanges();
        }
    }
}
