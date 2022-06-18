using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImHere.Services;
using ImHere.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImHere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly ReportingService _reportingService;
        private readonly EventService _eventService;

        public ReportController(ReportingService reportingService, EventService eventService)
        {
            _reportingService = reportingService;
            _eventService = eventService;
        }

        [HttpGet("student-list/{startString}/{endString}")]
        public async Task<IActionResult> GetStudentList (string startString, string endString, [FromQuery] int[] eventIds)
        {
            var students = await _reportingService.GetStudentOverviewReportDataAsync(ParseDateString(startString), ParseDateString(endString) + TimeSpan.FromHours(24), eventIds.ToList());

            var fileContent = "First Name,Last Name,Type,Methodist?,Local Church,Email,Phone Number,Check-In Count,First Check-In,Last Check-In\n";

            foreach (var student in students)
                fileContent += $"{student.FirstName},{student.LastName},{student.StudentType},{student.IsMethodist},{student.LocalChurch},{student.Email},{student.Phone},{student.CheckInCount},{(student.CheckInCount == 0 ? "" : student.FirstCheckIn.ToShortDateString())},{(student.CheckInCount == 0 ? "" : student.LastCheckIn.ToShortDateString())}\n";

            return File(Encoding.UTF8.GetBytes(fileContent), "text/csv", "studentlist.csv");
        }

        [HttpGet("checkins/{eventId:int}/{eventStartString}")]
        public async Task<IActionResult> GetCheckIns (int eventId, string eventStartString)
        {
            var eventStart = ParseDateString(eventStartString);
            var checkIns = await _reportingService.GetEventCheckInReportDataAsync(eventId, eventStart);
            var @event = await _eventService.GetEventAsync(eventId);

            var fileContent = $"{@event.Name} - {eventStart.ToShortDateString()}\nFirst Name,Last Name,Type,Email,Phone Number,Time Stamp\n";

            foreach (var checkIn in checkIns.OrderBy(c => c.Student.FirstName).OrderBy(c => c.Student.LastName))
                fileContent += $"{checkIn.Student.FirstName},{checkIn.Student.LastName},{checkIn.Student.StudentTypeDescription},{checkIn.Student.Email},{checkIn.Student.Phone},{checkIn.TimeStamp.ToShortTimeString()}\n";

            return File(Encoding.UTF8.GetBytes(fileContent), "text/csv", $"checkins.csv");
        }

        private DateTime ParseDateString(string dateString)
        {
            var dateSegments = dateString.Split('-');

            var parsedDate = dateSegments.Length == 3 ?
                new DateTime(int.Parse(dateSegments[2]), int.Parse(dateSegments[0]), int.Parse(dateSegments[1])) :
                new DateTime(int.Parse(dateSegments[2]), int.Parse(dateSegments[0]), int.Parse(dateSegments[1]), int.Parse(dateSegments[3]), int.Parse(dateSegments[4]), int.Parse(dateSegments[5]));

            return parsedDate;
        }
    }
}