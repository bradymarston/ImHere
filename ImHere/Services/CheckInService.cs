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
    public class CheckInService
    {
        private readonly CheckInRepository _checkInRepository;
        private readonly EventRepository _eventRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckInService(CheckInRepository checkInRepository, EventRepository eventRepository, IRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _checkInRepository = checkInRepository;
            _eventRepository = eventRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CheckInAsync(int eventId, int studentId)
            {
            var @event = await _eventRepository.GetAsync(eventId);
            if (@event is null)
                throw new KeyNotFoundException("Couldn't find the event to check in to.");

            var student = await _studentRepository.GetAsync(studentId);
            if (student is null)
                throw new KeyNotFoundException("Couldn't find student to check in.");

            var eventStartTime = @event.Schedule.GetStart(currentCentralTime);

            if ((await _checkInRepository.GetAsync(eventId, studentId, eventStartTime)) != null)
                throw new InvalidOperationException("Student cannot check in twice to the same event.");

            _checkInRepository.Add(
                new CheckIn
                {
                    Event = @event,
                    EventId = @event.Id,
                    Student = student,
                    StudentId = student.Id,
                    EventStart = eventStartTime,
                    TimeStamp = currentCentralTime,
                    IsAdminCheckIn = false
                });

            await _unitOfWork.CompleteAsync();
        }

        public async Task<CheckInDto> AdminCheckInAsync(int eventId, int studentId, DateTime date, bool useEventStartTimeInDb = true)
        {
            var @event = await _eventRepository.GetAsync(eventId);
            if (@event is null)
                throw new KeyNotFoundException("Couldn't find the event to check in to.");

            var student = await _studentRepository.GetAsync(studentId);
            if (student is null)
                throw new KeyNotFoundException("Couldn't find student to check in.");

            var eventStartTime = useEventStartTimeInDb ? date.Date + @event.Schedule.StartTime.TimeOfDay : date;

            if ((await _checkInRepository.GetAsync(eventId, studentId, eventStartTime)) != null)
                throw new InvalidOperationException("Student cannot check in twice to the same event.");

            var checkIn = new CheckIn
            {
                Event = @event,
                EventId = @event.Id,
                Student = student,
                StudentId = student.Id,
                EventStart = eventStartTime,
                TimeStamp = currentCentralTime,
                IsAdminCheckIn = true
            };

            _checkInRepository.Add(checkIn);

            await _unitOfWork.CompleteAsync();

            return checkIn.ToDto();
        }

        public async Task RemoveCheckInAsync(int checkInId)
        {
            var checkIn = await _checkInRepository.GetAsync(checkInId);
            _checkInRepository.Remove(checkIn);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CheckInDto>> GetCheckInsAsync(int eventId, DateTime eventStart, bool matchDateOnly = false)
        {
            if (matchDateOnly)
                return (await _checkInRepository.GetAsync(c => c.EventId == eventId && c.EventStart.Date == eventStart.Date)).ToList().ToDto();
            else
                return (await _checkInRepository.GetAsync(c => c.EventId == eventId && c.EventStart == eventStart)).ToList().ToDto();
        }

        private DateTime currentCentralTime
        {
            get
            {
                var currentUtcTime = DateTime.UtcNow;
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, timeZoneInfo);
            }
        }
    }
}