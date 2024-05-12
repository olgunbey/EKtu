using EKtu.Domain.Entities;
using EKtu.Repository.Constant;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IRepository.StudentRepository;
using ServiceStack;
using ServiceStack.Redis;
using System.Text.Json;

namespace EKtu.CacheService.CacheServices
{
    public class StudentCacheService : BaseCache, IStudentCacheService
    {
        IStudentRepository _studentRepository;
        public StudentCacheService(IStudentRepository studentRepository, IRedisClientAsync redisClient):base(redisClient)
        {
            _studentRepository = studentRepository;
            
        }
        public async Task<Dictionary<int, List<StudentChooseLessonCacheDto>>> AllStudentCacheLesson() 
        {
            Dictionary<int, List<StudentChooseLessonCacheDto>> keyValuePairs = new();
            string JsonDeserializerstring= await base.GetCache<string>(CacheConstant.StudentChooseLesson);
            if(JsonDeserializerstring is not null) //cachelenen datalar var 
            {
              var JsonDeser= JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonDeserializerstring);

                foreach (var item in JsonDeser)
                {
                 var studentId= item["Key"].ToString();
                 var Values= item["Value"].ToString();
                 var DeserializeDatas= JsonSerializer.Deserialize<List<StudentChooseLessonCacheDto>>(Values);

                 keyValuePairs.Add(Convert.ToInt16(studentId), DeserializeDatas);
                }
            }
            
            var studentChooseLessons= (await _studentRepository.AllStudentChooseLessonAsync());

            var  StudentChooseLessonStudentId= studentChooseLessons.Select(y => y.StudentId).Distinct().ToList();

            foreach (var studentId in StudentChooseLessonStudentId)
            {
                if (keyValuePairs.TryGetValue(studentId, out var Cachedata)) //bu kullanıcı cache'de var ayriyeten ders seçimlerini kontrol et
                {
                    var StudentsLessonIds = studentChooseLessons.Where(y => y.StudentId == studentId).Select(y => y.LessonId).ToList();

                    foreach (var studentlessonId in StudentsLessonIds)
                    {
                        if (Cachedata.Any(y => y.LessonCacheDtos.LessonId == studentlessonId))
                        {
                            //bu ders demekki cache'de mevcut
                        }
                        else
                        {
                            keyValuePairs.Remove(studentId);
                            var newCacheData= studentChooseLessons.Where(y => y.StudentId == studentId).Select(y => new StudentChooseLessonCacheDto()
                            {
                                LessonCacheDtos = new LessonCacheDto()
                                {
                                    LessonId = y.LessonId,
                                    LessonName = y.Lesson.LessonName
                                }
                            }).ToList() ;
                            keyValuePairs.Add(studentId, newCacheData);
                            break;
                            //bu ders ya değiştirildi, ya da yok
                        }
                    }
                }
                else
                {
                    keyValuePairs.Add(studentId,studentChooseLessons.Where(y => y.StudentId == studentId).Select(y => new StudentChooseLessonCacheDto()
                    {
                        LessonCacheDtos = new LessonCacheDto()
                        {
                            LessonId = y.LessonId,
                            LessonName = y.Lesson.LessonName
                        }
                    }).ToList());
                }

            }
            await base.SetCache(keyValuePairs, CacheConstant.StudentChooseLesson);
            return keyValuePairs;
        }

        public async Task AllStudentExamCache()
        {
            var queryAble= (await _studentRepository.AllStudentExamGrande()).ToList();

            var keyValuePairs = new Dictionary<int, List<AllStudentExamCacheDto>>();

            var y = queryAble.Select(y => y.StudentId).Distinct().ToList();

            foreach(var item in y)
            {
                var query = queryAble.Where(y => y.StudentId == item).ToList();


                
                keyValuePairs.Add(item, query.Where(y => y.ExamNote is not null).Select(p => new AllStudentExamCacheDto()
                {
                    Exam1 = p.ExamNote.Exam1,
                    Exam2 = p.ExamNote.Exam2,
                    StudentName = p.Student.FirstName,
                    LessonId = p.LessonId,
                    LessonName = p.Lesson.LessonName,
                    LetterGrade = p.ExamNote.LetterGrade
                }).ToList());
            }
            await base.SetCache(keyValuePairs, CacheConstant.StudentExam);
        }

        public async Task<Response<List<AllStudentExamCacheDto>>> GetAllStudentsExamCache(int userId)
        {
            string JsonSerializers= await base.GetCache<string>(CacheConstant.StudentExam);

            var dictionary =new Dictionary<int, List<AllStudentExamCacheDto>>();

            var JsonDeserializer= JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializers);

            foreach (var item in JsonDeserializer)
            {
              var key=  item["Key"].ToString();
              var value=  item["Value"].ToString();

              var JsonSerializervalue= JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(value);

                dictionary.Add(Convert.ToInt16(key), JsonSerializervalue);
            }

            if(dictionary.TryGetValue(userId, out var list))
            {
                return Response<List<AllStudentExamCacheDto>>.Success(list, 200);
            }
            return Response<List<AllStudentExamCacheDto>>.Fail("bu userId yok", 400);
        }

        public async Task<Response<List<GetStudentChooseLessonResponseDto>>> GetStudentCacheLesson(int studentId)
        {
            var cacheStudentChooseLesson= await base.GetCache<string>(CacheConstant.StudentChooseLesson);
            var keyValuePairs = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(cacheStudentChooseLesson);
            var result = new Dictionary<int, List<StudentChooseLessonCacheDto>>();

            foreach (var pair in keyValuePairs)
            {
                var Key = pair["Key"].ToString();
                var Values = pair["Value"].ToString();
                var DeserializeDatas= JsonSerializer.Deserialize<List<StudentChooseLessonCacheDto>>(Values);
                result.Add(Convert.ToInt16(Key), DeserializeDatas);
            }
            if (result.TryGetValue(studentId, out var student)) //demekki cachede bu kullanici var buradan cek
            {
                return Response<List<GetStudentChooseLessonResponseDto>>.Success(student.Select(y => new GetStudentChooseLessonResponseDto()
                {
                    LessonId = y.LessonCacheDtos.LessonId,
                    LessonName = y.LessonCacheDtos.LessonName
                }).ToList(), 200);
            }//demekki kullanıcı cache de degil bu kullanıcının derslerini önce bir cachle...
            return Response<List<GetStudentChooseLessonResponseDto>>.Fail("cachede lütfen kullanıcıyı cachle", 400);

        }
    }
}
