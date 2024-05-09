using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository.StudentRepository
{
    public interface IStudentRepository:IBaseRepository<Student>
    {
        Task<IQueryable<Student>> StudentListExamGrandeAsync(int studentId);
        Task StudentChooseLessonAsync(StudentChooseLessonRequestDto studentChooseLessonRequestDto);


        Task<Student> StudentCertificateAsync(int userId);
        Task<List<StudentAbsenceDto>> SelectingStudentAbsent(int userId);

        Task<IQueryable<StudentChooseLesson>> GetStudentChooseLessonAsync(int userId);
        Task<int> StudentChangeLessonAsync(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos, int studentId);
    }
}
