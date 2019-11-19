using ImHere.Data.Models;
using ImHere.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Mappers
{
    public static class EventMappers
    {
        public static EventDto ToDto(this Event @event)
        {
            if (@event is null)
                return null;
            
            var eventDto = new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
            };

            eventDto.Schedule = @event.Schedule switch
            {
                OneTimeScheduleInfo scheduleInfo => scheduleInfo.ToDto(),
                WeeklyScheduleInfo scheduleInfo => scheduleInfo.ToDto()
            };

            return eventDto;
        }

        public static IEnumerable<EventDto> ToDto(this IEnumerable<Event> events)
        {
            if (events is null)
                return null;

            return events.Select(e => e.ToDto()).ToList();
        }

        public static EventScheduleInfoDtoBase ToDto(this OneTimeScheduleInfo scheduleInfo)
        {
            if (scheduleInfo is null)
                return null;

            return new OneTimeScheduleInfoDto
            {
                StartTime = scheduleInfo.StartTime,
                Duration = scheduleInfo.Duration.TotalHours,
                Date = scheduleInfo.Date
            };
        }

        public static EventScheduleInfoDtoBase ToDto(this WeeklyScheduleInfo scheduleInfo)
        {
            if (scheduleInfo is null)
                return null;

            return new WeeklyScheduleInfoDto
            {
                StartTime = scheduleInfo.StartTime,
                Duration = scheduleInfo.Duration.TotalHours,
                Day = scheduleInfo.Day
            };
        }

        public static Event ToData(this EventDto eventDto)
        {
            if (eventDto is null)
                return null;

            var @event = new Event
            {
                Id = eventDto.Id,
                Name = eventDto.Name
            };

            @event.Schedule = eventDto.Schedule switch
            {
                OneTimeScheduleInfoDto scheduleInfoDto => scheduleInfoDto.ToData(@event),
                WeeklyScheduleInfoDto scheduleInfoDto => scheduleInfoDto.ToData(@event)
            };

            return @event;
        }

        public static EventScheduleInfoBase ToData(this OneTimeScheduleInfoDto scheduleInfoDto, Event @event)
        {
            if (scheduleInfoDto is null)
                return null;

            return new OneTimeScheduleInfo
            {
                EventId = @event.Id,
                Event = @event,
                Duration = TimeSpan.FromHours(scheduleInfoDto.Duration),
                StartTime = scheduleInfoDto.StartTime,
                Date = scheduleInfoDto.Date
            };
        }

        public static EventScheduleInfoBase ToData(this WeeklyScheduleInfoDto scheduleInfoDto, Event @event)
        {
            if (scheduleInfoDto is null)
                return null;

            return new WeeklyScheduleInfo
            {
                EventId = @event.Id,
                Event = @event,
                Duration = TimeSpan.FromHours(scheduleInfoDto.Duration),
                StartTime = scheduleInfoDto.StartTime,
                Day = scheduleInfoDto.Day
            };
        }

        public static void Update(this Event @event, EventDto eventDto, out EventScheduleInfoBase scheduleInfoToRemove)
        {
            scheduleInfoToRemove = null;

            if (@event is null)
                return;

            @event.Name = eventDto.Name;

            switch (eventDto.Schedule)
            {
                case OneTimeScheduleInfoDto scheduleInfoDto:
                    if (@event.Schedule.GetType() == typeof(OneTimeScheduleInfo))
                    {
                        ((OneTimeScheduleInfo)@event.Schedule).Update(scheduleInfoDto);
                    }
                    else
                    {
                        scheduleInfoToRemove = @event.Schedule;
                        @event.Schedule = scheduleInfoDto.ToData(@event);
                    }
                    break;
                case WeeklyScheduleInfoDto scheduleInfoDto:
                    if (@event.Schedule.GetType() == typeof(WeeklyScheduleInfo))
                    {
                        ((WeeklyScheduleInfo)@event.Schedule).Update(scheduleInfoDto);
                    }
                    else
                    {
                        scheduleInfoToRemove = @event.Schedule;
                        @event.Schedule = scheduleInfoDto.ToData(@event);
                    }
                    break;
            }
        }

        public static void Update(this OneTimeScheduleInfo scheduleInfo, OneTimeScheduleInfoDto scheduleInfoDto)
        {
            if (scheduleInfo is null)
                return;

            scheduleInfo.StartTime = scheduleInfoDto.StartTime;
            scheduleInfo.Duration = TimeSpan.FromHours(scheduleInfoDto.Duration);
            scheduleInfo.Date = scheduleInfoDto.Date;
        }

        public static void Update(this WeeklyScheduleInfo scheduleInfo, WeeklyScheduleInfoDto scheduleInfoDto)
        {
            if (scheduleInfo is null)
                return;

            scheduleInfo.StartTime = scheduleInfoDto.StartTime;
            scheduleInfo.Duration = TimeSpan.FromHours(scheduleInfoDto.Duration);
            scheduleInfo.Day = scheduleInfoDto.Day;
        }
    }
}