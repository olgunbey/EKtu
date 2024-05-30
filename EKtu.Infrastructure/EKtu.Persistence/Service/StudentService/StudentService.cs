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
        public StudentService(IBaseRepository<Student> baseRepository, ISaves saves, IStudentRepository studentRepository,IStudentBuilder _studentBuilder) : base(baseRepository, saves)
        {
            this.studentRepository = studentRepository;
            this._saves = saves;
            studentBuilder = _studentBuilder;
        }

        public async Task<Response<List<StudentListExamGrandeResponseDto>>> StudentListExamGrandeAsync(int studentId)
        {
         var IQueryAbleStudent= await studentRepository.StudentListExamGrandeAsync(studentId);

            var list =await IQueryAbleStudent.Select(y => new StudentListExamGrandeResponseDto()
            {
                FirstName=y.FirstName,
                LastName=y.LastName,
                StudentChooseExamGrande=y.LessonConfirmation.Select(x=> new StudentChooseLessonExamGrandeResponseDto()
                {
                    Exam1=x.ExamNote.Exam1,
                    Exam2=x.ExamNote.Exam2,
                    LessonName=x.Lesson.LessonName
                }).ToList(),
            }).ToListAsync();

            return Response<List<StudentListExamGrandeResponseDto>>.Success(list, 200);

        }


        public async Task<Response<int>> StudentCheckEmailAsync(string studentEmail)
        {
          Student student= await studentRepository.FirstOrDefaultAsync(y=>y.Email== studentEmail);
            if (student == null)
                return Response<int>.Fail("mail adresi bulunamadı", 404);

            return Response<int>.Success(student.Id, 200);

        }

        public async Task<Response<NoContent>> StudentChooseLessonAsync(StudentChooseLessonRequestDto studentChooseLessonRequestDto)
        {
            try
            {
                await studentRepository.StudentChooseLessonAsync(studentChooseLessonRequestDto);
                await _saves.SaveChangesAsync();
                return Response<NoContent>.Success(204);
            }
            catch (Exception)
            {
                return Response<NoContent>.Fail("Hata değer eklenemedi", 400);
            }
          
        }

        public async Task<Response<List<StudentAbsenceDto>>> StudentAbsenceAsync(int userId)
        {

           var query = (await studentRepository.SelectingStudentAbsent(userId)).ToList();
                if (query.Any())
                    return Response<List<StudentAbsenceDto>>.Success(query, 200);
            return Response<List<StudentAbsenceDto>>.Success(204);
        }

        public async Task<Response<StudentCertificateResponseDto>> StudentCertificateAsync(int userId)
        {
          Student? Student= await studentRepository.StudentCertificateAsync(userId);

            if (Student is null)
                return Response<StudentCertificateResponseDto>.Fail("öğrenci yok", 400);
           return Response<StudentCertificateResponseDto>.Success(new StudentCertificateResponseDto()
            {
                TckNo=Student.TckNo,
                FirstName=Student.FirstName,
                LastName=Student.LastName,
            },200);
        }

        public async Task<Response<List<GetStudentChooseLessonResponseDto>>> GetStudentChooseLessonAsync(int userId)
        {
         IQueryable<StudentChooseLesson> QueryableLessonConfirmation= await studentRepository.GetStudentChooseLessonAsync(userId);

            if (!QueryableLessonConfirmation.Any())
                return Response<List<GetStudentChooseLessonResponseDto>>.Fail("öğrenci ders seçmedi", 400);


                List<GetStudentChooseLessonResponseDto> getStudentChooseLessonDtos= await QueryableLessonConfirmation.Select(y => new GetStudentChooseLessonResponseDto()
            {
                LessonId = y.LessonId,
                LessonName = y.Lesson.LessonName
            }).ToListAsync();

            return Response<List<GetStudentChooseLessonResponseDto>>.Success(getStudentChooseLessonDtos, 200);

            
        }


        public async Task<Response<int>> StudentChangeLessonAsync(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos,int studentId)
        {
            try
            {
              int count=  await studentRepository.StudentChangeLessonAsync(studentChangeLessonRequestDtos, studentId);
                await _saves.SaveChangesAsync();
                return Response<int>.Success(count,204);
            }
            catch (Exception)
            {
                return Response<int>.Fail("hata, ders değişimi gerçekleşmedi", 400);
            }

        }

        public async Task<Response<int>> GetStudentClassIdWithStudentIdAsync(int studentId)
        {
          var student= await studentRepository.GetStudentClassIdWithStudentIdAsync(studentId);
            if(student is not Student)
            {
               return Response<int>.Fail("bu kullanıcı yok", 400);
            }
               return Response<int>.Success(student.ClassId, 200);
        }

        public async Task<Response<StudentInformationResponseDto>> StudentInformation(int studentId)
        {
          Student students= await studentRepository.StudentInformation(studentId);

            if(students is not Student)
            {
                throw new Exception("bu kullanıcı yok");
            }

           var responseData= new StudentInformationResponseDto()
            {
                ClassName = students.Class.ClassName,
                StudentName = students.FirstName + " " + students.LastName,
                ClassId= students.ClassId,
            };

            return Response<StudentInformationResponseDto>.Success(responseData, 200);
        }

        public async Task<Response<List<ClassListResponseDto>>> GetClassList()
        {
          var GetClasses=  (await studentRepository.GetAllClassList());


          var responseData= GetClasses.Select(y => new ClassListResponseDto()
            {
                ClassId = y.Id,
                ClassName = y.ClassName,
            }).ToList();
            return Response<List<ClassListResponseDto>>.Success(responseData, 200);
        }

        public async Task<Response<GetLessonResponseDto>> GetLessonTerm(TermLessonListRequestDto termLessonListRequestDto,int Grade)
        {
            var queryAbleData= await studentRepository.GetLessonTerm(termLessonListRequestDto, Grade);

            var SecmeliDersInts = queryAbleData.GroupBy(y => y.OptionalLessonId).Select(y => y.Key).ToList();

            SecmeliDersInts.Remove(null);



            GetLessonResponseDto respData=new GetLessonResponseDto();


            respData.AnaDers = queryAbleData.Where(y => y.OptionalLessonId == null).Select(y => new AnaDers()
            {
                LessonId = y.Id,
                LessonName = y.LessonName,
            }).ToList();
            respData.SecmeliDers = SecmeliDersInts.Count > 0 ? queryAbleData.Where(y => y.OptionalLessonId != null).Select(x => new SecmeliDers()
            {
                LessonId = x.Id,
                OptionalLessonId = (int)x.OptionalLessonId,
                LessonName = x.LessonName
            }).ToList() : null;



            
            return Response<GetLessonResponseDto>.Success(respData, 200);

            
          
        }
    }
}
