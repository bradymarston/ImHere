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
        public DbSet<ScheduledEvent> ScheduledEvents { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
