using ImHere.Data.Models;
using ImHere.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Mappers
{
    public static class CheckInMappers
    {
        public static CheckInDto ToDto(this CheckIn checkIn)
        {
            return new CheckInDto
            {
                Id = checkIn.Id,
                Student = checkIn.Student is null ? null : checkIn.Student.ToDto(),
                TimeStamp = checkIn.TimeStamp,
                EventStart = checkIn.EventStart,
                IsAdminCheckIn = checkIn.IsAdminCheckIn
            };
        }

        public static IEnumerable<CheckInDto> ToDto(this IEnumerable<CheckIn> checkIns)
        {
            return checkIns.Select(c => c.ToDto());
        }
    }
}
