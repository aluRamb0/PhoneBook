using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook.EfCore
{
    public abstract class DbContextBase : DbContext
    {
        protected DbContextBase() : base()
        { }

        protected DbContextBase(DbContextOptions options)
            : base(options)
        { }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        protected virtual void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var now = DateTimeOffset.UtcNow;
            foreach (var entry in entries)
            {
                var baseEntityName = entry.Entity.GetType().BaseType?.Name;
                if (string.IsNullOrEmpty(baseEntityName) || !baseEntityName.StartsWith(nameof(AuditedEntity<long>))) continue;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues[Constants.ModifiedDate] = now;
                        break;
                    case EntityState.Added:
                        entry.CurrentValues[Constants.CreatedDate] = now;
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues[Constants.DeletionDate] = now;
                        entry.CurrentValues[Constants.IsDeleted] = entry.State == EntityState.Deleted;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }
    }
}