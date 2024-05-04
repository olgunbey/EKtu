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

        public Task<IQueryable<Attendance>> SelectingStudentAbsent(int userId)
        {
           return Task.FromResult(_dbContext.Attendances.Include(y => y.StudentChooseLesson).ThenInclude(y=>y.Lesson)
                .Where(y => y.StudentChooseLesson.StudentId == userId));
        }

        public async Task<Student> StudentAbsenceAsync(int userId, int lessonId)
        {
            Student student= await _dbContext.FindAsync<Student>(userId);

            await _dbContext.Entry(student).Collection(y => y.StudentChooseLessons).LoadAsync();

            return student;
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
