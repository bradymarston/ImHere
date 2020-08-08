using ImHere.Data.Models;
using ImHere.Data.Repositories;
using ImHere.Services.Dtos;
using ImHere.Services.Exceptions;
using ImHere.Services.Mappers;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class StudentService
    {
        private readonly StudentRepository _studentRepository;
        private readonly IRepository<StudentType> _studentTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CheckInRepository _checkInRepository;

        //Filter predicates
        private Expression<Func<Student, bool>> MatchingNameFilter(StudentDto studentDto) => s => s.FirstName.ToUpper() == studentDto.FirstName.ToUpper() && s.LastName.ToUpper() == studentDto.LastName.ToUpper();

        public StudentService(StudentRepository studentRepository, IRepository<StudentType> studentTypeRepository, IUnitOfWork unitOfWork, CheckInRepository checkInRepository)
        {
            _studentRepository = studentRepository;
            _studentTypeRepository = studentTypeRepository;
            _unitOfWork = unitOfWork;
            _checkInRepository = checkInRepository;
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto studentDto)
        {
            if (studentDto is null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var duplicateStudents = await _studentRepository.GetAsync(MatchingNameFilter(studentDto));

            if (string.IsNullOrWhiteSpace(studentDto.Differentiator))
            {
                var baseStudent = duplicateStudents.FirstOrDefault(s => string.IsNullOrWhiteSpace(s.Differentiator));
                if (baseStudent != null)
                {
                    var baseStudentCheckIns = await _checkInRepository.GetAsync(c => c.Student.Id == baseStudent.Id);

                    if (baseStudentCheckIns.Count() > 0 && !baseStudentCheckIns.Any(c => !c.IsAdminCheckIn))
                    {
                        var updatedStudent = baseStudent.ToDto();
                        updatedStudent.FirstName = studentDto.FirstName;
                        updatedStudent.LastName = studentDto.LastName;
                        updatedStudent.IsMethodist = studentDto.IsMethodist;
                        updatedStudent.StudentTypeId = studentDto.StudentTypeId;
                        updatedStudent.Phone = studentDto.Phone;
                        updatedStudent.Email = studentDto.Email;

                        return await UpdateStudent(updatedStudent);
                    }
                }
            }

            if (duplicateStudents.Any(s => s.Differentiator == studentDto.Differentiator))
            {
                throw new DuplicateStudentException(duplicateStudents.Select(s => s.Differentiator));
            }

            var newStudent = studentDto.ToData();

            _studentRepository.Add(newStudent);

            await _unitOfWork.CompleteAsync();

            return newStudent.ToDto();
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
        {
            return (await _studentRepository.GetAsync()).ToDto();
        }

        public async Task<StudentDto> GetStudentAsync(int studentId)
        {
            return (await _studentRepository.GetAsync(studentId)).ToDto();
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsNotCheckedInAsync(int eventId, DateTime start)
        {
            return (await _studentRepository.GetNotCheckedInAsync(eventId, start)).ToDto();
        }

        public async Task SetMethodismAsync(int studentId, bool isMethodist)
        {
            var studentInDb = await GetStudentAsync(studentId);
            studentInDb.IsMethodist = isMethodist;
            await UpdateStudent(studentInDb);
        }

        public async Task SetLocalChurchAsync(int studentId, LocalChurch localChurch)
        {
            var studentInDb = await GetStudentAsync(studentId);
            studentInDb.LocalChurch = localChurch;
            await UpdateStudent(studentInDb);
        }

        public async Task<StudentDto> UpdateStudent(StudentDto studentDto)
        {
            if (studentDto is null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var studentInDb = await _studentRepository.GetAsync(studentDto.Id);

            if (studentInDb is null)
                throw new KeyNotFoundException("Couldn't find a student to update.");

            studentInDb.Update(studentDto);

            await _unitOfWork.CompleteAsync();

            return studentInDb.ToDto();
        }

        public async Task RemoveStudentAsync(int studentId)
        {
            var studentInDb = await _studentRepository.GetAsync(studentId);

            if (studentInDb is null)
                throw new KeyNotFoundException("Couldn't find a student to delete.");

            var studentCheckIns = await _checkInRepository.GetAsync(c => c.StudentId == studentId, false);

            if (studentCheckIns.Count() > 0)
                throw new InvalidOperationException("Cannot delete student with check-ins.");

            _studentRepository.Remove(studentInDb);

            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<StudentType>> GetStudentTypesAsync()
        {
            return await _studentTypeRepository.GetAsync();
        }
    }
}