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
        public int ScheduleId { get; set; }
        public ScheduleItemBase Schedule { get; set; } = new OneTimeScheduleItem();
    }
}
