using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class ScheduledEvent
    {
        public int ScheduledEventId { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool IsWeekly { get; set; }
    }
}
