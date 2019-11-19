using ImHere.Data.Models;
using ImHere.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services.Mappers
{
    public static class StudentMappers
    {
        public static StudentDto ToDto(this Student student)
        {
            if (student is null)
                return null;

            return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Phone = student.Phone,
                Email = student.Email
            };
        }

        public static IEnumerable<StudentDto> ToDto(this IEnumerable<Student> students)
        {
            return students.Select(s => s.ToDto()).ToList();
        }

        public static Student ToData(this StudentDto studentDto)
        {
            if (studentDto is null)
                return null;

            return new Student
            {
                Id = studentDto.Id,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Phone = studentDto.Phone,
                Email = studentDto.Email
            };
        }

        public static void Update(this Student student, StudentDto studentDto)
        {
            if (studentDto is null)
                return;

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Phone = studentDto.Phone;
            student.Email = studentDto.Email;
        }
    }
}
