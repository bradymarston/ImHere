using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class OneTimeScheduleInfo : EventScheduleInfoBase
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

        public override DateTime GetStart(DateTime testTime)
        {
            if (!IsHappening(testTime))
                throw new ArgumentOutOfRangeException(nameof(testTime), "The event is not occurring during the provided time.");

            return Date.Date + StartTime.TimeOfDay; 
        }
    }
}
