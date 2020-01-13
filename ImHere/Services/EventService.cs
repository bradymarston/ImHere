using ImHere.Data.Models;
using ImHere.Data.Repositories;
using ImHere.Services.Dtos;
using ImHere.Services.Mappers;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class EventService
    {
        private readonly EventRepository _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EventService(EventRepository eventRepository, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EventDto>> GetEventsAsync()
        {
            return (await _eventRepository.GetAsync()).ToDto();
        }

        public async Task<EventDto> GetEventAsync(int eventId)
        {
            return (await _eventRepository.GetAsync(eventId)).ToDto();
        }

        public async Task CreateEventAsync(EventDto eventDto)
        {
            if (eventDto is null)
            {
                throw new ArgumentNullException(nameof(eventDto));
            }

            if (string.IsNullOrEmpty(eventDto.Name))
            {
                throw new ArgumentException("A name must be provided.", nameof(eventDto));
            }

            if (eventDto.Schedule is null)
            {
                throw new ArgumentException("A schedule must be provided.", nameof(eventDto));
            }

            _eventRepository.Add(eventDto.ToData());
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateEventAsync(EventDto eventDto)
        {
            if (eventDto is null)
            {
                throw new ArgumentNullException(nameof(eventDto));
            }

            if (string.IsNullOrEmpty(eventDto.Name))
            {
                throw new ArgumentException("A name must be provided.", nameof(eventDto));
            }

            if (eventDto.Schedule is null)
            {
                throw new ArgumentException("A schedule must be provided.", nameof(eventDto));
            }

            var eventInDb = await _eventRepository.GetAsync(eventDto.Id);

            if (eventInDb is null)
            {
                throw new KeyNotFoundException("Couldn't find event to update.");
            }

            EventScheduleInfoBase scheduleInfoToRemove;

            eventInDb.Update(eventDto, out scheduleInfoToRemove);

            if (!(scheduleInfoToRemove is null))
                _eventRepository.RemoveScheduleInfo(scheduleInfoToRemove);

            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveEvent(EventDto eventDto)
        {
            if (eventDto is null)
            {
                throw new ArgumentNullException(nameof(eventDto));
            }

            var eventInDb = await _eventRepository.GetAsync(eventDto.Id);

            if (eventInDb is null)
            {
                throw new KeyNotFoundException("Couldn't find event to remove.");
            }

            var eventIdsWithCheckIns = await _eventRepository.GetEventIdsWithCheckIns();

            if (eventIdsWithCheckIns.Contains(eventDto.Id))
            {
                throw new InvalidOperationException("Cannot delete an event with check-ins.");
            }

            _eventRepository.Remove(eventInDb);

            await _unitOfWork.CompleteAsync(true);
        }

        public async Task<IEnumerable<EventDto>> GetHappeningEventsAsync()
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");            
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var events = (await _eventRepository.GetAsync()).Where(e => !e.Suspended);

            return events.Where(e => e.Schedule.IsHappening(currentTime)).ToDto();
        }

        public async Task<DateTime> GetCurrentStartTimeAsync(int eventId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var eventInDb = await _eventRepository.GetAsync(eventId);

            if (eventInDb is null)
                throw new KeyNotFoundException("Couldn't find event to get start time.");

            return eventInDb.Schedule.GetStart(currentTime);
        }

        public bool IsExpired(EventDto eventDto)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            return eventDto.ToData().Schedule.IsExpired(currentTime);
        }

        public async Task<IEnumerable<int>> GetEventIdsWithCheckIns()
        {
            return await _eventRepository.GetEventIdsWithCheckIns();
        }

        public async Task SetSuspensionStateAsync(EventDto eventDto, bool suspended)
        {
            if (eventDto is null)
            {
                throw new ArgumentNullException(nameof(eventDto));
            }

            var eventInDb = await _eventRepository.GetAsync(eventDto.Id);

            if (eventInDb is null)
            {
                throw new KeyNotFoundException("Couldn't find event to remove.");
            }

            eventInDb.Suspended = suspended;

            await _unitOfWork.CompleteAsync();
        }
    }
}
