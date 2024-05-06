using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.StudentRepository;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace EKtu.Persistence.Repository.StudentRepository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Task<List<StudentAbsenceDto>> SelectingStudentAbsent(int userId)
        {
          var Attendances=  _dbContext.Attendances.Include(y => y.StudentChooseLesson).ThenInclude(y => y.Lesson)
                .Where(y => y.StudentChooseLesson.StudentId == userId).ToList();
            List<StudentAbsenceDto> listAttendances=new List<StudentAbsenceDto>();
          var AbsentLessonsName=  Attendances.GroupBy(y => y.StudentChooseLesson.Lesson.LessonName).ToList();


            foreach (var item in AbsentLessonsName)
            {
               var attendance= Attendances.Where(y => y.StudentChooseLesson.Lesson.LessonName == item.Key).ToList();
                listAttendances.Add(new StudentAbsenceDto()
                {
                    LessonName = item.Key,
                    AbsenceCount = item.Count(),
                    AbsenceDateTimes=attendance.Select(y=>y.AttendanceDate).ToList()
                });
            }
            return Task.FromResult(listAttendances);
        }

        public async Task<Student> StudentCertificateAsync(int userId)
        {
           Student student= await _dbContext.Student.FindAsync(userId);
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
