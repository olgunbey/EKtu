using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Persistence.Builder.BuilderCreate;
using EKtu.Persistence.Builder.IBuilder;
using EKtu.Repository.Dtos;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.StudentRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.StudentService;
using Microsoft.EntityFrameworkCore;

namespace EKtu.Persistence.Service.StudentService
{
    public class StudentService : BaseService<Student>, IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private ISaves _saves;
        private readonly IStudentBuilder studentBuilder;
        public StudentService(IBaseRepository<Student> baseRepository, ISaves saves, IStudentRepository studentRepository) : base(baseRepository, saves)
        {
            this.studentRepository = studentRepository;
            this._saves = saves;
            studentBuilder = new StudentBuilder();
        }

        public async Task<Response<bool>> AddStudentHashPasswordAsync(StudentRequestDto studentRequestDto)
        {
          string HashedPassword= HashTransaction.HashPassword(studentRequestDto.StudentPassword);


          Student student= studentBuilder
                .FirstName(studentRequestDto.StudentName)
                .LastName(studentRequestDto.StudentLastName)
                .ClassId(studentRequestDto.ClassId)
                .Password(HashedPassword)
                .TckNo(studentRequestDto.StudentTckNo).Student();

            try //burayı bir transaction ile yönet
            {
                await studentRepository.AddAsync(student);
                await _saves.SaveChangesAsync();
                return Response<bool>.Success(true, 200);
            }
            catch (Exception)
            {
                return Response<bool>.Fail("hata öğrenci eklenemedi", 200);

                throw;
            }
        }

        public async Task<Response<List<StudentListExamGrandeResponseDto>>> StudentListExamGrandeAsync(int studentId)
        {
         var IQueryAbleStudent= await  studentRepository.StudentListExamGrandeAsync(studentId);

            var list =await IQueryAbleStudent.Select(y => new StudentListExamGrandeResponseDto()
            {
                FirstName=y.FirstName,
                LastName=y.LastName,
                StudentChooseExamGrande=y.StudentChooseLessons.Select(x=> new StudentChooseLessonExamGrandeResponseDto()
                {
                    Exam1=x.ExamNote.Exam1,
                    Exam2=x.ExamNote.Exam2,
                    LessonName=x.Lesson.LessonName
                }).ToList(),
            }).ToListAsync();

            return Response<List<StudentListExamGrandeResponseDto>>.Success(list, 200);

        }

        public async Task<Response<int>> StudentLogin(StudentLoginRequestDto studentLoginRequestDto)
        {
            Student? HasStudent = (await studentRepository.FindAllAsync(x => x.TckNo == studentLoginRequestDto.TckNo && x.Password == HashTransaction.HashPassword(studentLoginRequestDto.Password))).FirstOrDefault();
            if (HasStudent == null)
                return Response<int>.Fail("kullanıcı yok", 404);

            return Response<int>.Success(HasStudent.Id, 200);

        }

        public async Task<Response<int>> StudentCheckEmail(string studentEmail)
        {
          Student student= await studentRepository.FirstOrDefaultAsync(y=>y.Email== studentEmail);
            if (student == null)
                return Response<int>.Fail("mail adresi bulunamadı", 200);

            return Response<int>.Success(student.Id, 200);

        }
    }
}
