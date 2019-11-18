using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EventScheduleInfoDtoBase Schedule { get; set; } = new OneTimeScheduleInfoDto();
    }
}
