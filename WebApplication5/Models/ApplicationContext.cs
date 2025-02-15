using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplication5.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Person> Persons { get; set; } = null!;
    }
}
