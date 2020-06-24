using System;
using System.Collections;

namespace PhoneBook
{
    /// <summary>
    /// The base class for a model that needs to maintain auditing data.
    /// <remarks>Inspired by Entity Framework Core, shadow properties are created to monitor auditing information
    /// See documentation at https://docs.microsoft.com/en-us/ef/core/modeling/shadow-properties
    /// </remarks>
    /// </summary>
    /// <typeparam name="TPrimary">A <see cref="ValueType"/> that can be used a repository key field</typeparam>
    public abstract class AuditedEntity<TPrimary> : Entity<TPrimary> where TPrimary : struct
    {
        // ReSharper disable once RedundantBaseConstructorCall
        protected AuditedEntity():base()
        { }

        /// <summary>
        /// <remarks>This is intended to be used as an Entity Type Constructor by the underlying repository provider
        /// See documentation at https://docs.microsoft.com/en-us/ef/core/modeling/constructors
        /// </remarks>
        /// </summary>
        /// <param name="id">The database key value.</param>
        protected AuditedEntity(TPrimary id) : base(id)
        {}
    }
}
