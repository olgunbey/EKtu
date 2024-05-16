using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.ICacheService.StudentCacheService
{
    public interface IStudentCacheService
    {
        Task<Dictionary<int, List<StudentChooseLessonCacheDto>>> AllStudentCacheLesson();

        Task<Response<List<GetStudentChooseLessonResponseDto>>> GetStudentCacheLesson(int studentId);

        Task<Response<NoContent>> SetStudentCache(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,int classId);

        Task StudentNewExamGrande();

        Task<Response<List<CacheStudentExamListDto>>> GetCacheStudentGradeList(int classId, int studentId);


    }
}
