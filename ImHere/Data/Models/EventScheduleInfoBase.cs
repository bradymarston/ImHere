using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public abstract class EventScheduleInfoBase
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        [Column(TypeName = "bigint")]
        public TimeSpan Duration { get; set; }

        public abstract bool IsHappening(DateTime testTime);
        public abstract DateTime GetStart(DateTime testTime);
    }
}