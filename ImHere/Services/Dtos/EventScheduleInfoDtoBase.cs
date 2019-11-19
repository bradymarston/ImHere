using ImHere.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class EventScheduleInfoDtoBase
    {
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
    }
}
