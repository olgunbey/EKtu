using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.StudentRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EKtu.Persistence.Repository.StudentRepository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Task<IQueryable<StudentChooseLesson>> GetStudentChooseLessonAsync(int userId)
        {
           return Task.FromResult(_dbContext.StudentChooseLessons
                .Where(y => y.StudentId == userId)
                .Include(y => y.Student)
                .Include(y => y.Lesson).AsQueryable());
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
          return Task.FromResult(_dbContext.Student.Where(y=>y.Id==studentId).Include(y => y.LessonConfirmation)
                .ThenInclude(y => y.ExamNote)
                .Include(y => y.StudentChooseLessons)
                .ThenInclude(y => y.Lesson).AsQueryable());
        }

        public async Task<int> StudentChangeLessonAsync(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos,int studentId)
        {
             int count = 0;
            foreach (var studentChangeLessonRequestDto in studentChangeLessonRequestDtos)
            {
                var CurrentStudentLesson= await _dbContext.StudentChooseLessons.FirstAsync(y => y.LessonId == studentChangeLessonRequestDto.CurrentLessonId && y.StudentId==studentId);

                CurrentStudentLesson.LessonId = studentChangeLessonRequestDto.ChangeLessonId;
                count++;
            }
            return count;
        }

        public Task<IQueryable<StudentChooseLesson>> AllStudentChooseLessonAsync()
        {
           return Task.FromResult(_dbContext.StudentChooseLessons
                .Include(y => y.Lesson)
                .Include(y => y.Student).AsQueryable());
        }

        public Task<IQueryable<LessonConfirmation>> AllStudentExamGrande()
        {

           return Task.FromResult(_dbContext.LessonConfirmation.Include(y => y.Student)
                .Include(y => y.ExamNote)
                .Include(y=>y.Lesson)
                .AsQueryable());
        }

        public Task<IQueryable<Class>> ClassAllStudentExamGrandeList()
        {

           return Task.FromResult(_dbContext.Class.Include(y => y.Students)
               .ThenInclude(y => y.LessonConfirmation)
               .ThenInclude(y => y.Lesson)
               .ThenInclude(y => y.LessonConfirmation)
               .ThenInclude(y => y.ExamNote).AsQueryable());

        }

        public Task<IQueryable<Class>> GetClassList()
        {
            return Task.FromResult(_dbContext.Class.AsQueryable());
        }

        public async ValueTask<Student> GetStudentClassIdWithStudentIdAsync(int studentId)
        {
          return await _dbContext.Student.FindAsync(studentId);
        }
        public async Task<Student> StudentInformation(int userId)
        {
          return await _dbContext.Student.Include(y=>y.Class).FirstAsync(y=>y.Id==userId);

        }

        public Task<IQueryable<Class>> GetAllClassList()
        {
            return Task.FromResult(_dbContext.Class.AsQueryable());
        }
    }
}
