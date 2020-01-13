using ImHere.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            _context.Add(item);
        }

        public async Task<IEnumerable<Event>> GetAsync()
        {
            return await _context.Events.AddDefaultInclusions().ToListAsync();
        }

        public async Task<Event> GetAsync(int id)
        {
            return await _context.Events.AddDefaultInclusions().FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Remove(Event item)
        {
            _context.Remove(item);
        }

        public void RemoveScheduleInfo(EventScheduleInfoBase scheduleInfo)
        {
            _context.Remove(scheduleInfo);
        }

        public async Task<IEnumerable<int>> GetEventIdsWithCheckIns()
        {
            return await _context.Events.Where(e => e.CheckIns.Count() != 0).Select(e => e.Id).ToListAsync();
        }
    }

    public static class EventQueryExtensions
    {
        public static IIncludableQueryable<Event, EventScheduleInfoBase> AddDefaultInclusions(this IQueryable<Event> events)
        {
            return events.Include(e => e.Schedule);
        }
    }
}