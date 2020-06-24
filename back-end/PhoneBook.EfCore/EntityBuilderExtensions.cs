using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PhoneBook.EfCore
{
    public static class EntityBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> AddMetadataColumns<TEntity, TPrimary>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : Entity<TPrimary>
            where TPrimary : struct
        {
            //primary key
            builder.HasKey(p => p.Id);
            //creation metadata
            builder.Property<DateTimeOffset>(nameof(Constants.CreatedDate))
                .IsRequired();
            //modification metadata and concurrency token
            builder.Property<DateTimeOffset?>(nameof(Constants.ModifiedDate))
                .IsRequired(false)
                .IsConcurrencyToken();
            //deletion metadata
            builder.Property<bool>(nameof(Constants.IsDeleted))
                .HasDefaultValue(false)
                .IsRequired();
            builder.Property<DateTimeOffset?>(nameof(Constants.DeletionDate))
                .IsRequired(false);

            builder.HasQueryFilter(e => EF.Property<bool>(e, Constants.IsDeleted) == false);
            //singular table naming
            return builder.ToTable(typeof(TEntity).Name);
        }
    }
}