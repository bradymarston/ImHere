using ImHere.Data.Repositories;
using ImHere.Services.Dtos;
using ImHere.Services.Mappers;
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

        public ReportingService(CheckInRepository checkInService)
        {
            _checkInRepository = checkInService;
        }

        public async Task<IEnumerable<EventInstanceDto>> GetEventInstancesAsync()
        {
            var checkIns = await _checkInRepository.GetAsync();

            return checkIns.ToEventInstanceDto();
        }

        public async Task<int> GetCheckInCountAsync(DateTime start, DateTime end)
        {
            var checkIns = await _checkInRepository.GetAsync(c => c.TimeStamp >= start && c.TimeStamp <= end, false);

            return checkIns.Count();
        }

        public async Task<int> GetUniqueStudentCountAsync(DateTime start, DateTime end)
        {
            var checkIns = await _checkInRepository.GetAsync(c => c.TimeStamp >= start && c.TimeStamp <= end, false);

            return checkIns.GroupBy(c => c.StudentId).Count();
        }
    }
}
