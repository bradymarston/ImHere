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
                StudentTypeId = student.StudentTypeId,
                IsMethodist = student.IsMethodist,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Differentiator = student.Differentiator,
                Phone = student.Phone,
                Email = student.Email,
                StudentTypeDescription = student?.StudentType.Description
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
                StudentTypeId = studentDto.StudentTypeId,
                IsMethodist = studentDto.IsMethodist,
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Differentiator = studentDto.Differentiator,
                Phone = NormalizePhoneNumber(studentDto.Phone),
                Email = studentDto.Email
            };
        }

        public static void Update(this Student student, StudentDto studentDto)
        {
            if (studentDto is null)
                return;

            student.FirstName = studentDto.FirstName;
            student.LastName = studentDto.LastName;
            student.Differentiator = studentDto.Differentiator;
            student.Phone = NormalizePhoneNumber(studentDto.Phone);
            student.Email = studentDto.Email;
            student.StudentTypeId = studentDto.StudentTypeId;
            student.IsMethodist = studentDto.IsMethodist;
        }


        private static string NormalizePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;
            
            var phoneDigits = new string(phoneNumber.Where(c => c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9').ToArray());

            return phoneDigits.Length switch
            {
                10 => $"({phoneDigits.Substring(0, 3)}) {phoneDigits.Substring(3, 3)}-{phoneDigits.Substring(6, 4)}",
                7 => $"{phoneDigits.Substring(0, 3)}-{phoneDigits.Substring(3, 4)}",
                _ => phoneNumber
            };
        }
    }
}
