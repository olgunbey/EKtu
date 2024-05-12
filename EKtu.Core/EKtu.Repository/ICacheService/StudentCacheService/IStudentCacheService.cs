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
        Task AllStudentExamCache();

        Task<Response<List<AllStudentExamCacheDto>>> GetAllStudentsExamCache(int userId);


    }
}
