using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.TeacherService
{
    public interface ITeacherService:IBaseService<Teacher>
    {
        Task<Response<List<TeacherClassReponseDto>>> TeacherClass(int teacherId);
        Task<Response<List<TeacherLessonDto>>> TeacherLesson(int classId,int teacherId);

        Response<NoContent> EnteringStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,out List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos1);


        Task<Response<NoContent>> UpdateStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos);

        Task<Response<TeacherInformationDto>> TeacherInformation(int userId);

        Task<Response<List<GetAllTeacherResponseDto>>> GetAllTeacherAsync();

        Task<Response<List<GetAllStudentByClassIdAndLessonIdResponseDto>>> GetAllStudentByClassIdAndLessonId(GetAllStudentByClassIdAndLessonIdRequestDto getAllStudentByClassIdAndLessonIdRequestDto);

    }
}
