using System.Text.Json.Serialization;
using AutoMapper;

namespace PhoneBook.Services.Mapping
{
    public static class AutoMapperConfigExtensions
    {
        public static void CreateMappings(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<CreatePhoneBookDto, PhoneBook>()
                .ForMember(m => m.Id, options => options.Ignore());
            expression.CreateMap<PhoneBookDto, PhoneBook>()
                //The way AutoMapper maps collections create duplicate entries in the EfCore 3 ChangeTracker. Otherwise this would not be necessary.
                .ForMember(m => m.Entries, options => options.Ignore())
                
                .ReverseMap();

            expression.CreateMap<CreatePhoneBookDto, PhoneBookDto>();
            expression.CreateMap<EntryDto, Entry>()
                .ReverseMap();
        }
    }
}