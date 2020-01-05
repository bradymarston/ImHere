using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class EventInstanceDto
    {
        public string Name { get; set; }
        public int EventId { get; set; }
        public DateTime InstanceStart { get; set; }
        public IEnumerable<CheckInDto> CheckIns { get; set; }

        public EventInstanceDto()
        {
            CheckIns = new List<CheckInDto>();
        }
    }
}
