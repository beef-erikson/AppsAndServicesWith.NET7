using Microsoft.EntityFrameworkCore;
using Northwind.Console.HierarchyMapping;

DbContextOptionsBuilder<HierarchyDb> options = new();

// Modify this if you need to use Azure SQL Database or Edge
options.UseSqlServer("Data Source=.;Initial Catalog=HierarchyMapping;Integrated Security=true;TrustServerCertificate=true;");

using (HierarchyDb db = new(options.Options))
{
    bool deleted = await db.Database.EnsureDeletedAsync();
    WriteLine($"Database deleted: {deleted}");

    bool created = await db.Database.EnsureCreatedAsync();
    WriteLine($"Database created: {created}");

    WriteLine("SQL script used to create the database:");
    WriteLine(db.Database.GenerateCreateScript());

    // Students
    if (db.Students is null || db.Students.Count() == 0)
    {
        WriteLine("There are no students.");
    }
    else
    {
        foreach (Student student in db.Students) 
        {
            WriteLine("{0} studies {1}.",
                student.Name, student.Subject);
        }
    }

    // Employees
    if (db.Employees is null || db.Employees.Count() == 0)
    {
        WriteLine("There are no employees.");
    }
    else
    {
        foreach (Employee employee in db.Employees)
        {
            WriteLine("{0} was hired on {1}.",
                employee.Name, employee.HireDate);
        }
    }

    // People
    if (db.People is null || db.People.Count() == 0) 
    {
        WriteLine("There are no people.");
    }
    else
    {
        foreach (Person person in db.People)
        {
            WriteLine("{0} has ID of {1}.",
                person.Name, person.Id);
        }
    }
}