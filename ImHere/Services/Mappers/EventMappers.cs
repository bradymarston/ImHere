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
                RequireConfirmation = @event.RequireConfirmation,
                Suspended = @event.Suspended
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

            var scheduleInfoDto = new OneTimeScheduleInfoDto
            {
                Date = scheduleInfo.Date
            };

            ToDtoCommon(scheduleInfoDto, scheduleInfo);

            return scheduleInfoDto;
        }

        public static EventScheduleInfoDtoBase ToDto(this WeeklyScheduleInfo scheduleInfo)
        {
            if (scheduleInfo is null)
                return null;

            var scheduleInfoDto = new WeeklyScheduleInfoDto
            {
                Day = scheduleInfo.Day
            };

            ToDtoCommon(scheduleInfoDto, scheduleInfo);

            return scheduleInfoDto;
        }

        private static void ToDtoCommon(EventScheduleInfoDtoBase scheduleInfoDto, EventScheduleInfoBase scheduleInfo)
        {
            scheduleInfoDto.StartTime = scheduleInfo.StartTime;
            scheduleInfoDto.Duration = scheduleInfo.Duration.TotalHours;
        }

        public static Event ToData(this EventDto eventDto)
        {
            if (eventDto is null)
                return null;

            var @event = new Event
            {
                Id = eventDto.Id,
                Name = eventDto.Name,
                RequireConfirmation = eventDto.RequireConfirmation,
                Suspended = eventDto.Suspended
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

            var scheduleInfo = new OneTimeScheduleInfo
            {
                Date = scheduleInfoDto.Date
            };

            ToDataCommon(scheduleInfo, scheduleInfoDto, @event);

            return scheduleInfo;
        }

        public static EventScheduleInfoBase ToData(this WeeklyScheduleInfoDto scheduleInfoDto, Event @event)
        {
            if (scheduleInfoDto is null)
                return null;

            var scheduleInfo = new WeeklyScheduleInfo
            {
                Day = scheduleInfoDto.Day
            };

            ToDataCommon(scheduleInfo, scheduleInfoDto, @event);

            return scheduleInfo;
        }

        private static void ToDataCommon(EventScheduleInfoBase scheduleInfo, EventScheduleInfoDtoBase scheduleInfoDto, Event @event)
        {
            scheduleInfo.EventId = @event.Id;
            scheduleInfo.Event = @event;
            scheduleInfo.Duration = TimeSpan.FromHours(scheduleInfoDto.Duration);
            scheduleInfo.StartTime = scheduleInfoDto.StartTime;
        }

        public static void Update(this Event @event, EventDto eventDto, out EventScheduleInfoBase scheduleInfoToRemove)
        {
            scheduleInfoToRemove = null;

            if (eventDto is null)
                return;

            @event.Name = eventDto.Name;
            @event.RequireConfirmation = eventDto.RequireConfirmation;

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
            if (scheduleInfoDto is null)
                return;

            scheduleInfo.Date = scheduleInfoDto.Date;

            scheduleInfo.UpdateCommon(scheduleInfoDto);
        }

        public static void Update(this WeeklyScheduleInfo scheduleInfo, WeeklyScheduleInfoDto scheduleInfoDto)
        {
            if (scheduleInfoDto is null)
                return;

            scheduleInfo.Day = scheduleInfoDto.Day;

            scheduleInfo.UpdateCommon(scheduleInfoDto);
        }

        private static void UpdateCommon(this EventScheduleInfoBase scheduleInfo, EventScheduleInfoDtoBase scheduleInfoDto)
        {
            scheduleInfo.StartTime = scheduleInfoDto.StartTime;
            scheduleInfo.Duration = TimeSpan.FromHours(scheduleInfoDto.Duration);
        }
    }
}