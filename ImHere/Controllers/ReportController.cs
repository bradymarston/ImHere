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

        public ReportController(ReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        [HttpGet("student-list/{startString}/{endString}")]
        public async Task<IActionResult> GetStudentList (string startString, string endString)
        {
            var students = await _reportingService.GetStudentOverviewReportDataAsync(ParseDateString(startString), ParseDateString(endString) + TimeSpan.FromHours(24));

            var fileContent = "First Name,Last Name,Type,Email,Phone Number,Check-In Count,First Check-In,Last Check-In\n";

            foreach (var student in students)
                fileContent += $"{student.FirstName},{student.LastName},{student.StudentType},{student.Email},{student.Phone},{student.CheckInCount},{(student.CheckInCount == 0 ? "" : student.FirstCheckIn.ToShortDateString())},{(student.CheckInCount == 0 ? "" : student.LastCheckIn.ToShortDateString())}\n";

            return File(Encoding.UTF8.GetBytes(fileContent), "text/csv", "studentlist.csv");
        }

        private DateTime ParseDateString(string dateString)
        {
            var dateSegments = dateString.Split('-');

            return new DateTime(int.Parse(dateSegments[2]), int.Parse(dateSegments[0]), int.Parse(dateSegments[1]));
        }
    }
}