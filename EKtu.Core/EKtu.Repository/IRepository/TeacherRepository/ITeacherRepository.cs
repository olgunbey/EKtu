using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository.TeacherRepository
{
    public interface ITeacherRepository:IBaseRepository<Teacher>
    {
        Task<IQueryable<TeacherClassLesson>> TeacherClass(int teacherId);

        Task<IQueryable<TeacherClassLesson>> TeacherClassLesson(int teacherId, int classId);
        Task<List<Student>> StudentUpdateGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos);
        ValueTask<Teacher> TeacherInformation(int userId);

        Task<IQueryable<LessonConfirmation>> GetAllStudentByClassIdAndLessonIdAsync(GetAllStudentByClassIdAndLessonIdRequestDto getAllStudentByClassIdAndLessonIdRequestDto);
        void EnteringStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,out List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos1);


        Task<IQueryable<Student>> GetAllStudentExamNoteByClass(int classId,int lessonId);
    }
}
