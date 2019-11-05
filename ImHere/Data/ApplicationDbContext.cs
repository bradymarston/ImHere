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
        public DbSet<OneTimeEvent> OneTimeEventsItems { get; set; }
        public DbSet<WeeklyEvent> WeeklyEventsItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventBase>().HasKey(e => e.EventId);
            base.OnModelCreating(builder);
        }
    }
}
