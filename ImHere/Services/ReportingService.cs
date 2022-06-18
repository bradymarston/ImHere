using ImHere.Data.Repositories;
using ImHere.Services.Dtos;
using ImHere.Services.Mappers;
using ImHere.Services.Rptos;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class ReportingService
    {
        private readonly CheckInRepository _checkInRepository;
        private readonly StudentRepository _studentRepository;

        public ReportingService(CheckInRepository checkInService, StudentRepository studentRepository)
        {
            _checkInRepository = checkInService;
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<EventInstanceDto>> GetEventInstancesAsync()
        {
            var checkIns = await _checkInRepository.GetAsync();

            return checkIns.ToEventInstanceDto();
        }

        public async Task<IDictionary<string, int>> GetCheckInCountsAsync(DateTime start, DateTime end, IList<int> eventIds)
        {
            return await _checkInRepository.GetCheckInCountsAsync(c => c.EventStart >= start && c.EventStart <= end && eventIds.Contains(c.EventId));
        }

        public async Task<IDictionary<string, (int Total, int MethodistLocalChurch, int OtherLocalChurch)>> GetUniqueStudentCountsAsync(DateTime start, DateTime end, IList<int> eventIds)
        {
            return await _checkInRepository.GetUniqueStudentCountsAsync(c => c.EventStart >= start && c.EventStart <= end && eventIds.Contains(c.EventId));
        }

        public async Task<int> GetEventInstanceCountAsync(DateTime start, DateTime end, IList<int> eventIds)
        {
            return await _checkInRepository.GetEventInstanceCountAsync(c => c.EventStart >= start && c.EventStart <= end && eventIds.Contains(c.EventId));
        }

        public async Task<IEnumerable<StudentOverviewRpto>> GetStudentOverviewReportDataAsync(DateTime start, DateTime end, IList<int> eventIds = null)
        {
            return await _studentRepository.GetStudentReportDataAsync(start, end, eventIds);
        }

        public async Task<IEnumerable<CheckInDto>> GetEventCheckInReportDataAsync(int eventId, DateTime eventStart)
        {
            return (await _checkInRepository.GetAsync(c => c.EventId == eventId && c.EventStart == eventStart, true)).ToDto();
        }
    }
}
