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
        public DbSet<OneTimeScheduleItem> OneTimeScheduleItems { get; set; }
        public DbSet<WeeklyScheduleItem> WeeklyScheduleItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
