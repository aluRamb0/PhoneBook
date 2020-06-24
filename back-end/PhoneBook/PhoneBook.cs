using System;
using System.Collections.Generic;

namespace PhoneBook
{
    public class PhoneBook:AuditedEntity<Guid>
    {
        public string Name { get; set; }
        public ICollection<Entry> Entries { get; set; }
    }
}