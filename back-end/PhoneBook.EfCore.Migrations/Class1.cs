using System;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.EfCore.Migrations
{
    public class MigrationContext: PhoneBookContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            optionsBuilder.UseSqlite(Constants.DefaultConnectionString,
                options => options.MigrationsAssembly("PhoneBook.EfCore.Migrations"));
        }
    }
}