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
            return await _context.CheckIns.AddDefaultInclusions().ToListAsync();
        }

        public async Task<CheckIn> GetAsync(int id)
        {
            return await _context.CheckIns.AddDefaultInclusions().SingleOrDefaultAsync(c => c.Id == id);
        }

        public void Remove(CheckIn item)
        {
            _context.Remove(item);
        }
    }
    public static class CheckInQueryExtensions
    {
        public static IIncludableQueryable<CheckIn, Student> AddDefaultInclusions(this IQueryable<CheckIn> checkIns)
        {
            return checkIns.Include(c => c.Event).ThenInclude(e => e.Schedule).Include(c => c.Student);
        }
    }
}
