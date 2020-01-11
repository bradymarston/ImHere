using System;
using System.Collections.Generic;
using System.Text;
using ImHere.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImHere.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<OneTimeScheduleInfo> OneTimeScheduleItems { get; set; }
        public DbSet<WeeklyScheduleInfo> WeeklyScheduleItems { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<StudentType> StudentTypes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
