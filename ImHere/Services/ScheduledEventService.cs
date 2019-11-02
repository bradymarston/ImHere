using ImHere.Data.Models;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class ScheduledEventService
    {
        private readonly IRepository<ScheduledEvent> _eventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduledEventService(IRepository<ScheduledEvent> eventRepository, IUnitOfWork unitOfWork)
        {
            _eventRepository = eventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateEvent(ScheduledEvent newEvent)
        {
            _eventRepository.Add(newEvent);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ScheduledEvent>> GetEvents()
        {
            return await _eventRepository.GetAsync();
        }
    }
}
