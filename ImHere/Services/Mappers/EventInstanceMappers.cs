using ImHere.Data.Models;
using ImHere.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Mappers
{
    public static class EventInstanceMappers
    {
        public static IEnumerable<EventInstanceDto> ToEventInstanceDto(this IEnumerable<CheckIn> checkIns)
        {
            return checkIns.GroupBy(
                c => (c.EventId, c.EventStart), 
                c=> c, 
                (key, c) => new EventInstanceDto() 
                { 
                    EventId = key.EventId, 
                    InstanceStart = key.EventStart, 
                    Name = c.First().Event.Name,
                    CheckIns = checkIns.Where(c => c.EventId == key.EventId && c.EventStart == key.EventStart).ToDto()
                }).ToList();
        }
    }
}
