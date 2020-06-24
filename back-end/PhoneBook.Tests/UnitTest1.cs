using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using PhoneBook.EfCore;
using PhoneBook.Services;
using PhoneBook.Services.Mapping;
using Xunit;

namespace PhoneBook.Tests
{
    public class TestFixture
    {
        private ServiceProvider _sp;
        public ServiceProvider ServiceProvider => _sp ??= ConfigureServices().BuildServiceProvider();
        protected virtual Action<IMapperConfigurationExpression> MappingConfig =>
            AutoMapperConfigExtensions.CreateMappings;
        
        protected virtual IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services
                .AddDbContext<PhoneBookContext>(options =>
                {
                    if (options.IsConfigured) return;
                    options.UseInMemoryDatabase($"{GetType().Name}_Db");
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });
            if (MappingConfig != default)
            {
                services.AddAutoMapper(MappingConfig, MappingConfig.GetType().Assembly);
            }

            services.AddScoped<PhoneBookService>();
            return services;
        }
    }
    public class UnitTest1: IClassFixture<TestFixture>
    {
        private readonly PhoneBookService _service;
        public UnitTest1(TestFixture fixture)
        {
            _service = fixture.ServiceProvider.GetService<PhoneBookService>();
        }
        [Theory]
        [InlineData("Test Phone Book", "Test Entry", "09877867")]
        public async Task Can_Create(string name, string entryName, string number)
        {
            var id = await _service.CreateAsync(new CreatePhoneBookDto
            {
                Name = name,
                Entries = new List<EntryDto>
                {
                    new EntryDto
                    {
                        Name = entryName,
                        PhoneNumber = number
                    }
                }
            });
            Assert.NotEqual(default, id);
            Assert.True(await _service.PhoneBookExistsAsync(r=> r.Id == id));
        }
        [Theory]
        [InlineData("Test Phone Book", "Test Entry", "09877867", "000000")]
        public async Task Can_Update(string name, string entryName, string number, string expected)
        {
            var input = new CreatePhoneBookDto
            {
                Name = name,
                Entries = new List<EntryDto>
                {
                    new EntryDto
                    {
                        Name = entryName,
                        PhoneNumber = number
                    }
                }
            };
            var id = await _service.CreateAsync(input);
            Assert.NotEqual(default, id);
            Assert.True(await _service.PhoneBookExistsAsync(r=> r.Id == id));
            var token = JObject.FromObject(input);
            token.Add(nameof(id), id);
            var updateEntry = token.ToObject<PhoneBookDto>();
            var entryToUpdate = updateEntry.Entries.First();
            entryToUpdate.PhoneNumber = expected;
            await _service.UpdateAsync(updateEntry);
            var getResult = await _service.GetQuery().AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            Assert.NotNull(getResult);
            Assert.Single(getResult.Entries);
            var entry = getResult.Entries.First();
            Assert.Equal(expected, entry.PhoneNumber,StringComparer.InvariantCultureIgnoreCase);
        }
        
        [Theory]
        [InlineData("Test Phone Book", "Test Entry", "09877867")]
        public async Task Can_Delete(string name, string entryName, string number)
        {
            var id = await _service.CreateAsync(new CreatePhoneBookDto
            {
                Name = name,
                Entries = new List<EntryDto>
                {
                    new EntryDto
                    {
                        Name = entryName,
                        PhoneNumber = number
                    }
                }
            });
            Assert.NotEqual(default, id);
            Assert.True(await _service.PhoneBookExistsAsync(r=> r.Id == id));
            await _service.DeleteAsync(id);
            Assert.False(await _service.PhoneBookExistsAsync(r=> r.Id == id));
            
        }
    }
}