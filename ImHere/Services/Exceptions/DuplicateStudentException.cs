using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Exceptions
{
    public class DuplicateStudentException : Exception
    {
        public DuplicateStudentException() : base("Attempted to create a duplicate student")
        {

        }

        public DuplicateStudentException(IEnumerable<string> differentiatorsInUse) : base("Attempted to create a duplicate student")
        {
            DifferentiatorsInUse = differentiatorsInUse;
        }

        public IEnumerable<string> DifferentiatorsInUse { get; set; } = new List<string>();
    }
}
