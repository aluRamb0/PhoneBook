using System;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.EfCore
{
    public class PhoneBookContext:DbContextBase
    {
        public PhoneBookContext() : base()
        { }
        public PhoneBookContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entry>(builder =>builder.AddMetadataColumns<Entry, Guid>());
            modelBuilder.Entity<PhoneBook>(builder => builder.AddMetadataColumns<PhoneBook, Guid>());
        }
    }
}