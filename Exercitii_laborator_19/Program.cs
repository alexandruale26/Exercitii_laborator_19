using Data.Data;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;


#region Exercitiu
/*
    • Creati un proiectde tip asp.net web api

    • Creati modelul, populati DB
        • Adaugati PK, FK precum si relatiile necesare

    • Adaugati controllerul necesar(vedeti slide-ul urmator)

    • Adaugati endpoint-uri pentru
        • Obtinerea tuturor studentilor
        • Obtinerea unui student dupa ID
        • Creare student
        • Stergere student
        • Modificare date student
        • Modificare adresa student
            • In cazul in care studentul nu are adresa, aceasta va fi creata
        • Stergerea unui student
            • Cu un parametru care va specifica daca adresa este la randul ei stearsa

    • Suplimentar
        • Adaugati XML comments pentru endpoint-urile create
            • Folositi “Enable xml comments in swagger”
        • Folositi DTO-uri.
        • Folositi extension methods pentru a creea dto-uri
*/
#endregion


ResedDB();


DataLayer.CreateStudentWithAddress("Mara", "Danciu", 33, "Bucharest", "Traian", 23);
DataLayer.CreateStudentWithAddress("Mariana", "Marica", 29, "Brasov", "Cizmarului", 100);
DataLayer.CreateStudentWithAddress("Catalin", "Varan", 51, "Brasov", "Cazanului", 10);
DataLayer.CreateStudentWithAddress("Daniel", "Fastoc", 28, "Iasi", "Catedralei", 21);


using var context = new StudentsManagerContextDB();


context.SaveChanges();

DataLayer.ModifyStudentAddress(1, "dfd", null, null);
void ResedDB()
{
    using var context = new StudentsManagerContextDB();

    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();


    #region Addresses

    Address address1 = new Address
    {
        City = "Bucharest",
        Street = "Traian",
        StreetNumber = 23
    };

    Address address2 = new Address
    {
        City = "Bacau",
        Street = "Mioritei",
        StreetNumber = 2
    };

    Address address3 = new Address
    {
        City = "Bucharest",
        Street = "Decebal",
        StreetNumber = 134
    };

    Address address4 = new Address
    {
        City = "Iasi",
        Street = "Catedralei",
        StreetNumber = 21
    };

    #endregion


    #region Students

    context.Students.Add(new Student
    {
        FirstName = "Alexandra",
        LastName = "Stancu",
        Age = 34,
        Address = address1,
    });

    context.Students.Add(new Student
    {
        FirstName = "Gica",
        LastName = "Dobre",
        Age = 24
    });

    context.Students.Add(new Student
    {
        FirstName = "Mihaiela",
        LastName = "Morar",
        Age = 18
    });

    context.Students.Add(new Student
    {
        FirstName = "Daniel",
        LastName = "Ciobanu",
        Age = 27,
        Address = address3,
    });

    context.Students.Add(new Student
    {
        FirstName = "Culita",
        LastName = "Victor",
        Age = 44,
        Address = address2,
    });

    context.Students.Add(new Student
    {
        FirstName = "Ana",
        LastName = "Manole",
        Age = 37
    });

    context.Students.Add(new Student
    {
        FirstName = "Carmen",
        LastName = "Ciuca",
        Age = 22,
        Address = address4,
    });

    context.Students.Add(new Student
    {
        FirstName = "Bogdan",
        LastName = "Moraru",
        Age = 24
    });

    context.Students.Add(new Student
    {
        FirstName = "Crina",
        LastName = "Vlaicu",
        Age = 26,
        Address = address1,
    });

    context.Students.Add(new Student
    {
        FirstName = "Flavius",
        LastName = "Stancu",
        Age = 21,
        Address = address3,
    });

    #endregion

    context.SaveChanges();

}

