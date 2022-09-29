using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Data
{
    public class StudentsManagerContextDB: DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-42S4FFT\\SQLEXPRESS;Initial Catalog=StudentsManagerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

}
