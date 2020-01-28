using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Dtos
{
    public class CheckInDto
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime EventStart { get; set; }
        public bool IsAdminCheckIn { get; set; }
    }
}