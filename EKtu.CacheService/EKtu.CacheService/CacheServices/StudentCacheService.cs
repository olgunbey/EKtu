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
            List<StudentChooseLesson> studentChooseLessons= (await _studentRepository.AllStudentChooseLessonAsync()).ToList();

           var  StudentChooseLessonStudentId= studentChooseLessons.Select(y => y.StudentId).Distinct().ToList();

            foreach (var item in StudentChooseLessonStudentId)
            {
             var studentLessons= studentChooseLessons.Where(y => y.StudentId == item).ToList();
                keyValuePairs.Add(item,studentChooseLessons.Where(y => y.StudentId == item).Select(y => new StudentChooseLessonCacheDto()
                {
                    LessonCacheDtos = new()
                    {
                        LessonName=y.Lesson.LessonName,
                        LessonId=y.LessonId
                    }
                }).ToList());
            }
            await base.SetCache(keyValuePairs, CacheConstant.StudentChooseLesson);
            return keyValuePairs;
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
