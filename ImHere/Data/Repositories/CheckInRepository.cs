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
    public class CheckInRepository : IRepository<CheckIn>
    {
        private readonly ApplicationDbContext _context;

        public CheckInRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(CheckIn item)
        {
            _context.Add(item);
        }

        public async Task<IEnumerable<CheckIn>> GetAsync()
        {
            _context.Database.Migrate();
            return await _context.CheckIns.AddDefaultInclusions().ToListAsync();
        }

        public async Task<IEnumerable<CheckIn>> GetAsync(Expression<Func<CheckIn, bool>> predicate, bool includeChildren = true)
        {
            var query = _context.CheckIns.Where(predicate);

            if (includeChildren)
                query = query.AddDefaultInclusions();

            return await query.ToListAsync();
        }

        public async Task<CheckIn> GetAsync(int id)
        {
            return await _context.CheckIns.AddDefaultInclusions().SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CheckIn> GetAsync(int eventId, int studentId, DateTime start)
        {
            return await _context.CheckIns.AddDefaultInclusions().SingleOrDefaultAsync(c => c.EventId == eventId && c.StudentId == studentId && c.EventStart == start);
        }

        public void Remove(CheckIn item)
        {
            _context.Remove(item);
        }
    }

    public static class CheckInQueryExtensions
    {
        public static IIncludableQueryable<CheckIn, StudentType> AddDefaultInclusions(this IQueryable<CheckIn> checkIns)
        {
            return checkIns.Include(c => c.Event).ThenInclude(e => e.Schedule).Include(c => c.Student).ThenInclude(s => s.StudentType);
        }
    }
}
