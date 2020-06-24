using System;

namespace PhoneBook
{
    /// <summary>
    /// The base class for a model that needs to be store in a repository.
    /// </summary>
    /// <typeparam name="TPrimary">A <see cref="ValueType"/> for the key field</typeparam>
    public abstract class Entity<TPrimary> where TPrimary: struct
    {
        public Entity()
        { }
        /// <summary>
        /// <remarks>This is intended to be used as an Entity Type Constructor by the underlying repository provider
        /// See documentation at https://docs.microsoft.com/en-us/ef/core/modeling/constructors
        /// </remarks>
        /// </summary>
        /// <param name="id">The database key value.</param>
        public Entity(TPrimary id) => Id = id;
        /// <summary>
        /// The repository key.
        /// <remarks>Inspired by Entity Framework Core, this is intended to be used as an Entity Type Constructor. The Database provider will own the responsibility of setting this value
        /// See documentation at https://docs.microsoft.com/en-us/ef/core/modeling/constructors
        /// </remarks>
        /// </summary>
        public virtual TPrimary Id { get; set; }
    }
}