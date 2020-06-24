using System.Collections.Generic;

namespace PhoneBook.Services.Mapping
{
    public class CreatePhoneBookDto
    {
        public string Name { get; set; }
        public List<EntryDto> Entries { get; set; } = new List<EntryDto>();
    }
}