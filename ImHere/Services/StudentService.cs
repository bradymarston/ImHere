using ImHere.Data.Models;
using ImHere.Services.Dtos;
using ImHere.Services.Mappers;
using ShadySoft.EntityPersistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImHere.Services
{
    public class StudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto studentDto)
        {
            if (studentDto is null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var newStudent = studentDto.ToData();

            _studentRepository.Add(newStudent);

            await _unitOfWork.CompleteAsync();

            return newStudent.ToDto();
        }

        public async Task<IEnumerable<StudentDto>> GetStudents()
        {
            return (await _studentRepository.GetAsync()).ToDto();
        }

        public async Task<StudentDto> GetStudent(int studentId)
        {
            return (await _studentRepository.GetAsync(studentId)).ToDto();
        }

        public async Task UpdateStudent(StudentDto studentDto)
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
        }

        public async Task RemoveStudent(StudentDto studentDto)
        {
            if (studentDto is null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var studentInDb = await _studentRepository.GetAsync(studentDto.Id);

            if (studentInDb is null)
                throw new KeyNotFoundException("Couldn't find a student to delete.");

            _studentRepository.Remove(studentInDb);

            await _unitOfWork.CompleteAsync();
        }
    }
}