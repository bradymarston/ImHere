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
        private readonly CheckInRepository _checkInService;

        public ReportingService(CheckInRepository checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<IEnumerable<EventInstanceDto>> GetEventInstancesAsync()
        {
            var checkIns = await _checkInService.GetAsync();

            return checkIns.ToEventInstanceDto();
        }
    }
}
