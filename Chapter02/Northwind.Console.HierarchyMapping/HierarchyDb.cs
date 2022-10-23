using Microsoft.EntityFrameworkCore;

namespace Northwind.Console.HierarchyMapping
{
    internal class HierarchyDb : DbContext
    {
        public DbSet<Person>? People { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Employee>? Employees { get; set;}

        public HierarchyDb(DbContextOptions<HierarchyDb> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                // .UseTphMappingStrategy();
                // .UseTptMappingStrategy();
                .UseTpcMappingStrategy()
                .Property(person => person.Id)
                .HasDefaultValueSql("NEXT VALUE FOR [PersonIds]");

            modelBuilder.HasSequence<int>("PersonIds", builder =>
            {
                builder.StartsAt(4);
            });

            // Sample data
            Student p1 = new() { Id = 1, Name = "Beef Erikson", Subject = "Science" };
            Employee p2 = new() { Id = 2, Name = "Robert Farlow", HireDate = new(year: 2020, month: 9, day: 3) };
            Employee p3 = new() { Id = 3, Name = "Linda Buttress", HireDate = new(year: 1998, month: 6, day: 4) };

            modelBuilder.Entity<Student>().HasData(p1);
            modelBuilder.Entity<Employee>().HasData(p2, p3);
        }
    }
}
