using ImHere.Data.Models;
using ImHere.Data.Repositories;
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
        private readonly IUnitOfWork _unitOfWork;

        public CheckInService(CheckInRepository checkInRepository, IUnitOfWork unitOfWork)
        {
            _checkInRepository = checkInRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CheckInAsync(Event @event, Student student)
        {
            var currentUtcTime = DateTime.UtcNow;
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var currentCentralTime = TimeZoneInfo.ConvertTimeFromUtc(currentUtcTime, timeZoneInfo);

            var eventStartTime = @event.Schedule.GetStart(currentCentralTime);

            _checkInRepository.Add(new CheckIn
            {
                Event = @event,
                EventId = @event.Id,
                Student = student,
                StudentId = student.Id,
                EventStart = eventStartTime,
                TimeStamp = currentUtcTime
            });

            await _unitOfWork.CompleteAsync();
        }
    }
}
