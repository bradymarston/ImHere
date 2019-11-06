using ImHere.Data.Models;
using Microsoft.EntityFrameworkCore;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImHere.Data.Repositories
{
    public class EventRepository : IRepository<Event>
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Event item)
        {
            _context.Events.Add(item);
        }

        public async Task<IEnumerable<Event>> GetAsync()
        {
            return await _context.Events.Include(DefaultInclusion).ToListAsync();
        }

        public async Task<Event> GetAsync(int id)
        {
            return await _context.Events.Include(DefaultInclusion).FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Remove(Event item)
        {
            _context.Events.Remove(item);
        }

        private readonly Expression<Func<Event, EventScheduleInfoBase>> DefaultInclusion = (e) => e.Schedule;
    }
}
