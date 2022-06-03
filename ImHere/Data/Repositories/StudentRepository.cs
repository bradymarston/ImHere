using ImHere.Data.Models;
using ImHere.Services.Rptos;
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
    public class StudentRepository : IRepository<Student>
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Student item)
        {
            _context.Add(item);
        }

        public async Task<IEnumerable<Student>> GetAsync()
        {
            return await _context.Students.AddDefaultInclusions().ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> filterPredicate)
        {
            return await _context.Students.AddDefaultInclusions().Where(filterPredicate).ToListAsync();
        }

        public async Task<Student> GetAsync(int id)
        {
            return await _context.Students.AddDefaultInclusions().SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetNotCheckedInAsync(int eventId, DateTime start)
        {
            return await _context.Students.AddDefaultInclusions().Where(s => !s.CheckIns.Any(c => c.EventId == eventId && c.EventStart == start)).ToListAsync();
        }

        public async Task<IEnumerable<StudentOverviewRpto>> GetStudentReportDataAsync(DateTime startDate, DateTime endDate, IList<int> eventIds = null)
        {
            return await _context.Students
                .Where(s => s.CheckIns.Count() == 0 || s.CheckIns.Any(c => c.TimeStamp >= startDate && c.TimeStamp <= endDate && (eventIds == null || eventIds.Contains(c.EventId))))
                .Select(s =>
                new StudentOverviewRpto()
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Differentiator = s.Differentiator,
                    StudentType = s.StudentType.Description,
                    Email = s.Email,
                    Phone = s.Phone,
                    IsMethodist = s.IsMethodist,
                    CheckInCount = s.CheckIns.Count(),
                    FirstCheckIn = s.CheckIns.Min(c => c.TimeStamp),
                    LastCheckIn = s.CheckIns.Max(c => c.TimeStamp)
                }
            ).ToListAsync();
        }

        public void Remove(Student item)
        {
            _context.Remove(item);
        }
    }
    public static class StudentQueryExtensions
    {
        public static IIncludableQueryable<Student, StudentType> AddDefaultInclusions(this IQueryable<Student> students)
        {
            return students.Include(s => s.StudentType);
        }
    }
}
