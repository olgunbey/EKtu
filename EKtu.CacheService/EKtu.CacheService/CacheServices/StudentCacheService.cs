using EKtu.Domain.Entities;
using EKtu.Repository.Constant;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IRepository.StudentRepository;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.SystemJson;
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
                if (keyValuePairs.TryGetValue(studentId, out var Cachedata))
                {
                    var StudentsLessonIds = studentChooseLessons.Where(y => y.StudentId == studentId).Select(y => y.LessonId).ToList();

                    foreach (var studentlessonId in StudentsLessonIds)
                    {
                        if (!Cachedata.Any(y => y.LessonCacheDtos.LessonId == studentlessonId))
                        {
                            keyValuePairs.Remove(studentId);
                            var newCacheData = studentChooseLessons.Where(y => y.StudentId == studentId).Select(y => new StudentChooseLessonCacheDto()
                            {
                                LessonCacheDtos = new LessonCacheDto()
                                {
                                    LessonId = y.LessonId,
                                    LessonName = y.Lesson.LessonName
                                }
                            }).ToList();
                            keyValuePairs.Add(studentId, newCacheData);
                            break;
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
           string JsonSerializerData= await base.GetCache<string>(CacheConstant.StudentExam);

            var keyValuePairs = new Dictionary<int, List<AllStudentExamCacheDto>>();

            if (JsonSerializerData != null)//cache'de değer var 
            {
                var JsonDeserializerCache = JsonSerializer.Deserialize<List<Dictionary<string,object>>>(JsonSerializerData);
                foreach (var item in JsonDeserializerCache)
                {
                    var key = item["Key"].ToString();
                    var Values = item["Value"].ToString();
                    var JsonDeserializeExamGrande = JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(Values);

                    if(JsonDeserializeExamGrande.Any())
                    keyValuePairs.Add(Convert.ToInt16(key), JsonDeserializeExamGrande);
                }
            }


            var queryAble= (await _studentRepository.AllStudentExamGrande()).ToList();

            var DatabaseStudentId = queryAble.Select(y => y.StudentId).Distinct().ToList();

            var DatabaseStudentLessonId=queryAble.Where(y=> DatabaseStudentId.Any(x=>x==y.StudentId)).Select(y=>y.LessonId).Distinct().ToList();

            foreach(var item in DatabaseStudentId)
            {
                foreach (var itemLessonId in DatabaseStudentLessonId)
                {
                     var xy=  queryAble.Where(y => y.Lesson.Id == itemLessonId && y.StudentId == item).First();

                    if (keyValuePairs.TryGetValue(item, out var CacheData))
                    {
                        if(xy.ExamNote is not null)
                        {
                            var dto = CacheData.Where(y => y.LessonId == itemLessonId).First();

                            if (dto.Exam1 != xy.ExamNote.Exam1 || dto.Exam2 != xy.ExamNote.Exam2)
                            {
                                // yani burada not değişmiş
                                keyValuePairs.Remove(item);

                                var cacheDtos = queryAble.Where(y => y.StudentId == item && y.ExamNote is not null).Select(y => new AllStudentExamCacheDto()
                                {
                                    Exam1 = y.ExamNote.Exam1,
                                    Exam2 = y.ExamNote.Exam2,
                                    LessonId = y.LessonId,
                                    LessonName = y.Lesson.LessonName,
                                    LetterGrade = y.ExamNote.LetterGrade!
                                }).ToList();
                                keyValuePairs.Add(item, cacheDtos);
                                break;
                            }
                        }
                    }
                    else
                    {
                        var cacheDtos = queryAble.Where(y => y.StudentId == item && y.ExamNote is not null).Select(y => new AllStudentExamCacheDto()
                        {
                            Exam1 = y.ExamNote.Exam1,
                            Exam2 = y.ExamNote.Exam2,
                            LessonId = y.LessonId,
                            LessonName = y.Lesson.LessonName,
                            LetterGrade = y.ExamNote.LetterGrade!
                        }).ToList();
                        if(cacheDtos.Any())
                        keyValuePairs.Add(item, cacheDtos);
                        break;

                    }
                }
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
            }
            return Response<List<GetStudentChooseLessonResponseDto>>.Fail("cachede lütfen kullanıcıyı cachle", 400);

        }
    }
}
