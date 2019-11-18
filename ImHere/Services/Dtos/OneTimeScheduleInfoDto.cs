using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class OneTimeScheduleInfoDto : EventScheduleInfoDtoBase
    {
        public DateTime Date { get; set; }
    }
}
