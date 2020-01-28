using ImHere.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Rptos
{
    public class StudentOverviewRpto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Differentiator { get; set; }
        public string StudentType { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CheckInCount { get; set; }
        public DateTime FirstCheckIn { get; set; }
        public DateTime LastCheckIn { get; set; }

        public string DisplayName => $"{FirstName} {LastName}" + (string.IsNullOrWhiteSpace(Differentiator) ? "" : $" ({Differentiator})");
    }
}
