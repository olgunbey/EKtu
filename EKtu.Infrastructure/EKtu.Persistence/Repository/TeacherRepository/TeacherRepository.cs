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
                LessonConfirmation? lessonConfirmation =  _dbContext.LessonConfirmation.FirstOrDefault(y => y.LessonId == item.LessonId && y.StudentId == item.StudentId); //öğrencinin bu derse kaydını kontrol eder 

                if(lessonConfirmation is null)
                {
                    return;
                }
                else
                {
                    _dbContext.LessonConfirmation.Entry(lessonConfirmation).Reference(y => y.ExamNote).Load();

                    if(lessonConfirmation.ExamNote is null) //yani burada ögrencinin ders kaydı var ancak veritabanında examnote tablosunda notu yok
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
                        //demekki veritabanında ögrencinin notu var, bu yüzden güncelleme olacak
                        outenteringStudentGradesRequestDtos.Add(item);
                    }
                }
                
            }
        }

        public async Task<IQueryable<LessonConfirmation>> GetAllStudentByClassIdAndLessonIdAsync(GetAllStudentByClassIdAndLessonIdRequestDto getAllStudentByClassIdAndLessonIdRequestDto)
        {

           return _dbContext.LessonConfirmation.Include(y => y.Student).AsNoTrackingWithIdentityResolution()
                .Include(y => y.ExamNote)
                .Where(y => y.Student.ClassId == getAllStudentByClassIdAndLessonIdRequestDto.ClassId && y.LessonId==getAllStudentByClassIdAndLessonIdRequestDto.LessonId).AsQueryable();

        }

        public Task<IQueryable<Student>> GetAllStudentExamNoteByClass(int classId, int lessonId)
        {
            return Task.FromResult(_dbContext.Student
        .AsNoTrackingWithIdentityResolution()
        .Where(y => y.ClassId == classId)
        .Include(y=>y.LessonConfirmation)
        .ThenInclude(y=>y.ExamNote)
        .Include(y=>y.LessonConfirmation)
        .ThenInclude(y=>y.Lesson)
        .Select(student => new
        {
            Student = student,
            LessonConfirmations = student.LessonConfirmation
                .Where(lc => lc.LessonId == lessonId)
                .ToList()
        })
        .AsEnumerable()
        .Select(studentWithFilteredConfirmations => {
            studentWithFilteredConfirmations.Student.LessonConfirmation = studentWithFilteredConfirmations.LessonConfirmations;
            return studentWithFilteredConfirmations.Student;
        })
        .AsQueryable()
            ); 
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

        public async Task<List<LessonConfirmation>> TestRepository(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos)
        {
            List<LessonConfirmation> lessonConfirmations=new List<LessonConfirmation>();

            foreach (var item in enteringStudentGradesRequestDtos)
            {
              LessonConfirmation lessonConfirmation=await _dbContext.LessonConfirmation.FirstAsync(y => y.StudentId == item.StudentId && y.LessonId == item.LessonId);
                await _dbContext.Entry(lessonConfirmation).Reference(y => y.Lesson).LoadAsync();
                await _dbContext.Entry(lessonConfirmation).Reference(y=>y.ExamNote).LoadAsync();

                lessonConfirmations.Add(lessonConfirmation);
            }
            return lessonConfirmations;
        }
    }
}
