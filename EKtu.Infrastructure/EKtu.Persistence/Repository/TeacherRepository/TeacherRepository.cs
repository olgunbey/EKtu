using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository.TeacherRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace EKtu.Persistence.Repository.TeacherRepository
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Teacher> EmailAndPassword(Expression<Func<Teacher, bool>> expression)
        {
          return await _dbContext.Set<Teacher>().FirstOrDefaultAsync(expression);
        }

        public void EnteringStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,out List<EnteringStudentGradesRequestDto> outenteringStudentGradesRequestDtos)
        {
            outenteringStudentGradesRequestDtos = new();
            foreach (var item in enteringStudentGradesRequestDtos)
            {
                LessonConfirmation lessonConfirmation =  _dbContext.LessonConfirmation.First(y => y.LessonId == item.LessonId && y.StudentId == item.StudentId);

              var deneme=  _dbContext.ExamNote.Select(y => y.LessonConfirmationId).ToList();
                 _dbContext.LessonConfirmation.Entry(lessonConfirmation).Reference(y => y.ExamNote).Load();
                if (lessonConfirmation.ExamNote is null)
                {
                    _dbContext.ExamNote.Add(new ExamNote()
                    {
                       Exam1 = item.Exam_1,
                       Exam2 = item.Exam_2,
                       LessonConfirmationId = lessonConfirmation.Id,
                    });
                }
                else
                {
                    outenteringStudentGradesRequestDtos.Add(item);
                }
                
            }
        }

        public async Task<List<Student>> StudentUpdateGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos)
        {
            List<Student> students = new List<Student>();
            foreach (var item in enteringStudentGradesRequestDtos)
            {
              Student? student= await _dbContext.Student.FindAsync(item.StudentId);

               await _dbContext.Student.Entry(student!)
                    .Collection(y => y.LessonConfirmation)
                    .Query()
                    .Where(y=>y.LessonId==item.LessonId)
                    .Include(y => y.ExamNote)
                    .LoadAsync();

              var st =  student.LessonConfirmation.Where(y => y.LessonId == item.LessonId).Select(y => y.ExamNote).First();
                if (st.Exam1!=item.Exam_1 || st.Exam2!=item.Exam_2)
                {
                    st.Exam2=item.Exam_2;
                    st.Exam1 = item.Exam_1;
                    students.Add(student);
                }
            }
            return students;
            
        }

        public Task<IQueryable<TeacherClassLesson>> TeacherClass(int teacherId)
        {
          return Task.FromResult(_dbContext.Set<TeacherClassLesson>().Where(y => y.TeacherId == teacherId).Include(y => y.Teacher).Include(y=>y.Class).Include(t=>t.Lesson).AsQueryable());
        
        }

        public Task<IQueryable<TeacherClassLesson>> TeacherClassLesson(int teacherId, int classId)
        {
           return Task.FromResult(_dbContext.Set<TeacherClassLesson>().Where(y => y.TeacherId == teacherId && y.ClassId == classId)
                .Include(y => y.Lesson).AsQueryable());
        }

        public async ValueTask<Teacher> TeacherInformation(int userId)
        {
           return await _dbContext.Teacher.FindAsync(userId);
        }
    }
}
