using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsAdminCheckIn { get; set; }
    }
}