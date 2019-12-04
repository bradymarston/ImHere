using ImHere.Data.Models;
using Microsoft.EntityFrameworkCore;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetAsync(int id)
        {
            return await _context.Students.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetNotCheckedInAsync(int eventId, DateTime start)
        {
            return await _context.Students.Where(s => !s.CheckIns.Any(c => c.EventId == eventId && c.EventStart == start)).ToListAsync();
        }

        public void Remove(Student item)
        {
            _context.Remove(item);
        }
    }
}
