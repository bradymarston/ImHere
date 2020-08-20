using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public EventScheduleInfoBase Schedule { get; set; } = new OneTimeScheduleInfo();
        public bool RequireConfirmation { get; set; }
        public bool Suspended { get; set; }
        public IEnumerable<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    }
}
