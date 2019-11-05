﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public abstract class EventBase
    {
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }

        public abstract bool IsHappening(DateTime testTime);
    }
}
