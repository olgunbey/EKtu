using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.StudentService
{
    public interface IStudentService:IBaseService<Student> 
    {
        Task<Response<NoContent>> AddStudentHashPasswordAsync(StudentRequestDto studentRequestDto);

        Task<Response<List<StudentListExamGrandeResponseDto>>> StudentListExamGrandeAsync(int studentId);

        Task<Response<int>> StudentLogin(StudentLoginRequestDto studentLoginRequestDto);

        Task<Response<int>> StudentCheckEmail(string studentEmail);
    }
}
