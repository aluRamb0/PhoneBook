using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBook.EfCore;
using PhoneBook.Services.Mapping;

namespace PhoneBook.Services
{
    public class PhoneBookService
    {
        
        public PhoneBookService(PhoneBookContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }
        public PhoneBookContext Context { get;  }
        public IMapper Mapper { get; }

        public IQueryable<PhoneBook> GetQuery() => Context.Set<PhoneBook>().Include(r => r.Entries);

        public async Task<bool> PhoneBookExistsAsync(Expression<Func<PhoneBook, bool>> expression)
        {
            return await GetQuery().Where(expression).AnyAsync();
        }
        public async Task<bool> EntryExistsAsync(Expression<Func<Entry, bool>> expression)
        {
            return await Context.Set<Entry>().Where(expression).AnyAsync();
        }
        
        public async Task<Guid> CreateAsync(CreatePhoneBookDto input)
        {
            var record = Mapper.Map<PhoneBook>(input);
            var entry = await Context.Set<PhoneBook>().AddAsync(record);
            await Context.SaveChangesAsync();
            return entry.Entity.Id;
        }

        public async Task UpdateAsync(PhoneBookDto input)
        {
            var record = await GetQuery().FirstOrDefaultAsync(r => r.Id == input.Id);
            Mapper.Map(input, record);
            record.Entries = Map(record,input.Entries);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var record = await GetQuery().FirstOrDefaultAsync(r => r.Id == id);
            Context.Set<Entry>().RemoveRange(record.Entries);
            Context.Set<PhoneBook>().RemoveRange(record);
            await Context.SaveChangesAsync();

        }

        public async Task DeleteEntryAsync(Guid id)
        {
            var record = await Context.Set<Entry>().FirstOrDefaultAsync(r => r.Id == id);
            Context.Set<Entry>().RemoveRange(record);
            await Context.SaveChangesAsync();
        }
            

        /// <summary>
        ///  Maps source records with existing records
        /// <remarks>
        ///  The way AutoMapper maps collections create duplicate entries in the EfCore 3 ChangeTracker. Otherwise this would not be necessary.
        /// </remarks>
        /// </summary>
        /// <param name="record"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        private IList<Entry> Map(PhoneBook record, IEnumerable<EntryDto> entries)
        {
            return entries.GroupJoin(record.Entries, source => source.Id, target => target.Id,
                    (source, target) => new {source, target})
                .SelectMany(gj => gj.target.DefaultIfEmpty(),
                    (gj, entry) => entry == null ? Mapper.Map<Entry>(gj.source) : Mapper.Map(gj.source, entry))
                .ToList();

        }
    }
}