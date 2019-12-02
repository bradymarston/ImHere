using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class WeeklyScheduleInfo : EventScheduleInfoBase
    {
        [Required]
        public DayOfWeek Day { get; set; }

        public override bool IsHappening(DateTime testTime)
        {
            var testTimeInWeek = TimeSpan.FromDays((double)testTime.DayOfWeek) + testTime.TimeOfDay;
            var startTimeInWeek = TimeSpan.FromDays((double)Day) + StartTime.TimeOfDay;
            var endTimeInWeek = startTimeInWeek + Duration;

            var weekLength = TimeSpan.FromDays(7);

            if (endTimeInWeek < weekLength)
                return testTimeInWeek >= startTimeInWeek && testTimeInWeek <= endTimeInWeek;
            else
                return testTimeInWeek >= startTimeInWeek || testTimeInWeek <= endTimeInWeek - weekLength;
        }

        public override DateTime GetStart(DateTime testTime)
        {
            if (!IsHappening(testTime))
                throw new ArgumentOutOfRangeException(nameof(testTime), "The event is not occurring during the provided time.");

            var occasionDate = testTime.Date;

            while (occasionDate.DayOfWeek != Day)
                occasionDate -= TimeSpan.FromDays(1);

            return occasionDate.Date + StartTime.TimeOfDay;
        }
    }
}