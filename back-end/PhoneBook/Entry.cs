using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook
{
    public class Entry: AuditedEntity<Guid>
    {
        public string Name { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}