using System;

namespace PhoneBook.Services.Mapping
{
    public class PhoneBookDto : CreatePhoneBookDto
    {
        public Guid Id { get; set; }
    }
}