using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.StudentRepository;
using Microsoft.EntityFrameworkCore;

namespace EKtu.Persistence.Repository.StudentRepository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task StudentChooseLessonAsync(StudentChooseLessonRequestDto studentChooseLessonRequestDto)
        {

         List<StudentChooseLesson> studentChooseLesson= studentChooseLessonRequestDto.LessonId.Select(y =>new StudentChooseLesson()
            {
                StudentId=studentChooseLessonRequestDto.StudentId,
                LessonId=y
            }).ToList();
               await _dbContext.StudentChooseLessons.AddRangeAsync(studentChooseLesson);
        }

        public Task<IQueryable<Student>> StudentListExamGrandeAsync(int studentId)
        {
          return Task.FromResult(_dbContext.Student.Where(y=>y.Id==studentId).Include(y => y.StudentChooseLessons)
                .ThenInclude(y => y.ExamNote)
                .Include(y => y.StudentChooseLessons)
                .ThenInclude(y => y.Lesson).AsQueryable());
        }
    }
}
