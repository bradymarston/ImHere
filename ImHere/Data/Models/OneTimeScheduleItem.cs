using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class OneTimeScheduleItem : ScheduleItemBase
    {
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        public override bool IsHappening(DateTime testTime)
        {
            var compositeStart = Date.Date + StartTime.TimeOfDay;
            var compositeEnd = compositeStart + Duration;

            var normalizedTestTime = testTime.Date + testTime.TimeOfDay;

            return normalizedTestTime >= compositeStart && normalizedTestTime <= compositeEnd;
        }
    }
}
