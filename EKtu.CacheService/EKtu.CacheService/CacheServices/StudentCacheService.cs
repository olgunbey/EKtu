﻿using EKtu.Domain.Entities;
using EKtu.Repository.Constant;
using EKtu.Repository.Dtos;
using EKtu.Repository.ICacheService.StudentCacheService;
using EKtu.Repository.IRepository;
using EKtu.Repository.IRepository.StudentRepository;
using EKtu.Repository.IRepository.TeacherRepository;
using EKtu.Repository.IService.StudentService;
using EKtu.Repository.IService.TeacherService;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Script;
using ServiceStack.SystemJson;
using System.Text.Json;

namespace EKtu.CacheService.CacheServices
{
    public class StudentCacheService : BaseCache, IStudentCacheService
    {
        IStudentRepository _studentRepository;
        ITeacherRepository _teacherRepository;
        public StudentCacheService(IStudentRepository studentRepository, IRedisClientAsync redisClient, ITeacherRepository teacherRepository) : base(redisClient)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
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


        public async Task StudentUpdateExamNote(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,int classId)
        {
            var CacheStudentExamListByClassAndLesson = await base.GetCache<string>(CacheConstant.StudentExam);
            var keyValuePairs = new Dictionary<int, List<AllStudentExamCacheDto>>();

            if (CacheStudentExamListByClassAndLesson is not null)  //demekki cache'nin içi dolu
            {
                //bu if'te cache içini dolduruyoruz
                List<AllStudentExamCacheDto> allStudentExamCacheDtos = new();
                var JsonSerializeDatas = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(CacheStudentExamListByClassAndLesson);


                foreach (var item in JsonSerializeDatas)
                {
                    string key = item["Key"].ToString();
                    string Value = item["Value"].ToString();

                    var JsonDeserializers = JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(Value);
                    keyValuePairs.Add(Convert.ToInt16(key), JsonDeserializers);
                }

                AllStudentExamCacheDto2 tmp;
                if(keyValuePairs.TryGetValue(classId, out var data)) //burada mutlaka zaten if'in içine girecek cünkü eklenmeden güncellenemez!!! eklenen notlar her türlü cache'de var!!
                {
                    foreach (var item in enteringStudentGradesRequestDtos)
                    {

                        var StudentCache=  data.Single(y => y.StudentId == item.StudentId);
                        var studentLessonExamCache= StudentCache.AllStudentExamCacheDtos.Single(y=>y.LessonId== item.LessonId);

                        tmp=studentLessonExamCache;

                        data.Remove(StudentCache);
                        StudentCache.AllStudentExamCacheDtos.Remove(studentLessonExamCache);

                        StudentCache.AllStudentExamCacheDtos.Add(new AllStudentExamCacheDto2()
                        {
                            Exam1=item.Exam_1,
                            Exam2=item.Exam_2,
                            LessonId=item.LessonId,
                            LessonName=tmp.LessonName,
                            LetterGrade=tmp.LetterGrade,
                            Term=tmp.Term,
                        });
                        data.Add(StudentCache);
                    }
                }
               await this.SetCache(keyValuePairs, CacheConstant.StudentExam);

            }
            else //eğer cache boş ise veritabanından bütün notları çekip cachler
            {
                var AllStudentExamGrande = (await _studentRepository.ClassAllStudentExamGrandeList()).ToList();
                var KeyValuePair = new Dictionary<int, List<AllStudentExamCacheDto>>();
                var AllClassList = (await _studentRepository.GetClassList()).ToList();

                foreach (var item in AllClassList)
                {
                    var CacheDto = AllStudentExamGrande.Where(y => y.Id == item.Id).SelectMany(y => y.Students).Select(y => new AllStudentExamCacheDto()
                    {
                        StudentId = y.Id,
                        AllStudentExamCacheDtos = y.LessonConfirmation.Where(y => y.ExamNote is not null).Select(y => new AllStudentExamCacheDto2()
                        {
                            Exam1 = y.ExamNote.Exam1,
                            Exam2 = y.ExamNote.Exam2,
                            LessonId = y.LessonId,
                            LetterGrade = y.ExamNote.LetterGrade!,
                            LessonName = y.Lesson.LessonName,
                            Term = y.Lesson.Term,
                        }).ToList(),
                        StudentName = y.FirstName
                    }).ToList();
                    KeyValuePair.Add(item.Id, CacheDto);
                }
                await base.SetCache(KeyValuePair, CacheConstant.StudentExam);

            }
              



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

        public async Task<Response<NoContent>> SetStudentCache(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,int classId)
        {

            var JsonSerializerCacheData= await base.GetCache<string>(CacheConstant.StudentExam);
            if(JsonSerializerCacheData is null)
            {

                var dictionary = new Dictionary<int,List<AllStudentExamCacheDto>>();
                var resp= (await _teacherRepository.GetAllStudentExamNoteByClass(classId, enteringStudentGradesRequestDtos.Select(y => y.LessonId).First())).ToList();

                dictionary.Add(classId, resp.Select(y => new AllStudentExamCacheDto()
                {
                    StudentId = y.Id,
                    StudentName=y.FirstName+ ""+y.LastName,
                    AllStudentExamCacheDtos=y.LessonConfirmation.Select(x=> new AllStudentExamCacheDto2()
                    {
                        Exam1=x.ExamNote.Exam1,
                        Exam2 = x.ExamNote.Exam2,
                        LessonId=x.LessonId,
                        LessonName=x.Lesson.LessonName,
                        Term=x.Lesson.Term
                    }).ToList()
                }).ToList());

                await base.SetCache(dictionary,CacheConstant.StudentExam);



                return Response<NoContent>.Success(203);
            }
            var keyValuePairs = new Dictionary<int, List<AllStudentExamCacheDto>>();
            List<AllStudentExamCacheDto> allStudentExamCacheDtos = new();
         var JsonSerializeDatas= JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializerCacheData);


            foreach (var item in JsonSerializeDatas)
            {
                string key= item["Key"].ToString();
                string Value= item["Value"].ToString();

                var JsonDeserializers= JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(Value);
                keyValuePairs.Add(Convert.ToInt16(key), JsonDeserializers);
            }

            foreach (var item in enteringStudentGradesRequestDtos)
            {
                if (keyValuePairs.TryGetValue(classId,out var CacheData))
                {
                  var Student= CacheData.First(y => y.StudentId == item.StudentId);

                  var studentsLEsson= Student.AllStudentExamCacheDtos.FirstOrDefault(y => y.LessonId == item.LessonId); //bu ders yoksa cachlenecek

                    if (studentsLEsson == null)
                    {
                       await base.SetCache(keyValuePairs, CacheConstant.StudentExam);
                    }

                    if(studentsLEsson.Exam1 != item.Exam_1 || studentsLEsson.Exam2!=item.Exam_2)
                    {
                        studentsLEsson.Exam1 = item.Exam_1;
                        studentsLEsson.Exam2 = item.Exam_2;
                        keyValuePairs.Remove(classId);
                        keyValuePairs.Add(classId, CacheData);
                    }
                }
            }
           await base.SetCache(keyValuePairs, CacheConstant.StudentExam);
           return Response<NoContent>.Success(203);
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
            if (result.TryGetValue(studentId, out var student)) 
            {
                return Response<List<GetStudentChooseLessonResponseDto>>.Success(student.Select(y => new GetStudentChooseLessonResponseDto()
                {
                    LessonId = y.LessonCacheDtos.LessonId,
                    LessonName = y.LessonCacheDtos.LessonName
                }).ToList(), 200);
            }
            return Response<List<GetStudentChooseLessonResponseDto>>.Fail("cachede lütfen kullanıcıyı cachle", 400);
        }

        public async Task<Response<List<CacheStudentExamListDto>>> GetCacheStudentGradeList(int classId, int studentId, bool term)
        {
         string JsonSerializerData=  await base.GetCache<string>(CacheConstant.StudentExam);

            if(JsonSerializerData is null)
            {
                return Response<List<CacheStudentExamListDto>>.Fail("cache boşş", 400);
            }
            var keyValuePair = new Dictionary<int, List<AllStudentExamCacheDto>>();

            var BaseJsonDeserializeDatas= JsonSerializer.Deserialize<List<Dictionary<string, object>>>(JsonSerializerData);

            foreach (var item in BaseJsonDeserializeDatas)
            {
                string Key= item["Key"].ToString();
                var Values = item["Value"].ToString();

                 var JsonData= JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(Values);

                keyValuePair.Add(Convert.ToInt32(Key), JsonData);
            }

            if(keyValuePair.TryGetValue(classId,out var CacheData))
            {
              AllStudentExamCacheDto currentStudentExam= CacheData.First(y => y.StudentId == studentId);
               

              var responsedata= currentStudentExam.AllStudentExamCacheDtos.Where(y=>y.Term==term).Select(y => new CacheStudentExamListDto()
                {
                    LessonId = y.LessonId,
                    LessonName = y.LessonName,
                    LetterGrade = y.LetterGrade,
                    Exam1 = y.Exam1,
                    Exam2 = y.Exam2
                }).ToList();

                return Response<List<CacheStudentExamListDto>>.Success(responsedata, 200);
            }
            return Response<List<CacheStudentExamListDto>>.Fail("bu sınıf yok", 400);
        }



        public async Task TestCache(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,int classId)
        {
            var CacheStudentExamListByClassAndLesson = await base.GetCache<string>(CacheConstant.StudentExam);
            var keyValuePairs = new Dictionary<int, List<AllStudentExamCacheDto>>();

            if (CacheStudentExamListByClassAndLesson is not null)  //demekki cache'nin içi dolu
            {
                //bu if'te cache içini dolduruyoruz
                List<AllStudentExamCacheDto> allStudentExamCacheDtos = new();
                var JsonSerializeDatas = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(CacheStudentExamListByClassAndLesson);


                foreach (var item in JsonSerializeDatas)
                {
                    string key = item["Key"].ToString();
                    string Value = item["Value"].ToString();

                    var JsonDeserializers = JsonSerializer.Deserialize<List<AllStudentExamCacheDto>>(Value);
                    keyValuePairs.Add(Convert.ToInt16(key), JsonDeserializers);
                }

             var listLessonConfirmation= await  _teacherRepository.TestRepository(enteringStudentGradesRequestDtos);//burada yalnızca veritabanına ekledigim examnote'leri aldım


                if(keyValuePairs.TryGetValue(classId,out var data))
                {
                    foreach (var item in enteringStudentGradesRequestDtos)
                    {
                     var datas = data.FirstOrDefault(y => y.StudentId == item.StudentId);

                        if(datas is not null && !datas.AllStudentExamCacheDtos.Any(y=>y.LessonId==item.LessonId))//yani bu ögrencinin aslında diger dersleri rediste var, ancak bu notu değiştirilen dersi yok
                        {
                         var dbStudentExamNote= listLessonConfirmation.First(y => y.StudentId == item.StudentId && y.LessonId==item.LessonId);


                            datas.AllStudentExamCacheDtos.Add(new AllStudentExamCacheDto2() //burada da bu öğrencinin db'ye eklenen examnote'si cache eklenir
                            {
                                Exam1 = dbStudentExamNote.ExamNote.Exam1,
                                Exam2 = dbStudentExamNote.ExamNote.Exam2,
                                LessonName = dbStudentExamNote.Lesson.LessonName,
                                LessonId = dbStudentExamNote.LessonId,
                                LetterGrade = dbStudentExamNote.ExamNote.LetterGrade,
                                Term = dbStudentExamNote.Lesson.Term
                            });

                            keyValuePairs.Remove(classId);

                            data.Add(datas);

                            keyValuePairs.Add(classId, data);
                        }

                    }
                   await this.SetCache(keyValuePairs, CacheConstant.StudentExam);

                }

            }
            else
            {
                var AllStudentExamGrande = (await _studentRepository.ClassAllStudentExamGrandeList()).ToList();
                var AllClassList = (await _studentRepository.GetClassList()).ToList();

                foreach (var item in AllClassList)
                {
                    var CacheDto = AllStudentExamGrande.Where(y => y.Id == item.Id).SelectMany(y => y.Students).Select(y => new AllStudentExamCacheDto()
                    {
                        StudentId = y.Id,
                        AllStudentExamCacheDtos = y.LessonConfirmation.Where(y => y.ExamNote is not null).Select(y => new AllStudentExamCacheDto2()
                        {
                            Exam1 = y.ExamNote.Exam1,
                            Exam2 = y.ExamNote.Exam2,
                            LessonId = y.LessonId,
                            LetterGrade = y.ExamNote.LetterGrade!,
                            LessonName = y.Lesson.LessonName,
                            Term = y.Lesson.Term,
                        }).ToList(),
                        StudentName = y.FirstName
                    }).ToList();
                    keyValuePairs.Add(item.Id, CacheDto);
                }
                await base.SetCache(keyValuePairs, CacheConstant.StudentExam);
            }
            //1) ilk başta cachedeki verileri al  +
            //2) ardından cachedeki aldıgımız verilere yeni eklenen examnote'leri ekle
            //3) ardından son halini cachle


        }
        
    }
}
