using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImHere.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImHere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly StudentService _studentService;

        public ReportController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("student-list")]
        public async Task<IActionResult> GetStudentList ()
        {
            var students = await _studentService.GetStudentsAsync();

            var fileContent = "First Name,Last Name,Phone Number,Email\n";

            foreach (var student in students)
                fileContent += $"{student.FirstName},{student.LastName},{student.Phone},{student.Email}\n";

            return File(Encoding.UTF8.GetBytes(fileContent), "text/csv", "studentlist.csv");
        }
    }
}