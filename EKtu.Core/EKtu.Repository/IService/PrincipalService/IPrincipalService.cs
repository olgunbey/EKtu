using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.PrincipalService
{
    public interface IPrincipalService:IBaseService<Principal>
    {
        Task<Response<NoContent>> StudentChooseApproveAsync();
        Task<Response<NoContent>> AddLessonAsync(AddLessonRequestDto addLessonRequestDto);
        Task<Response<NoContent>> AddTeacherClassLessonAsync(AddTeacherClassLessonRequestDto addTeacherClassLessonRequestDto);
        Task<Response<NoContent>> StudentCalculateLetterGrandeAsync(StudentCalculateLetterGrandeDto studentCalculateLetterGrandeDto);

        Task<Response<NoContent>> AllStudentCalculateLetterGrandeAsync();
    }
}
