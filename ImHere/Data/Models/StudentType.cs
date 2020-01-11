using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Data.Models
{
    public class StudentType
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IList<Student> Students { get; set; } = new List<Student>();
    }
}
