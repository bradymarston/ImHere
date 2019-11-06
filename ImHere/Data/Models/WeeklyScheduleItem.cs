using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class WeeklyScheduleItem : ScheduleItemBase
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
    }
}