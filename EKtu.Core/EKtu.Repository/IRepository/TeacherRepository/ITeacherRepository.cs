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
        Task StudentUpdateGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos);
        ValueTask<Teacher> TeacherInformation(int userId);
        void EnteringStudentGrades(List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos,out List<EnteringStudentGradesRequestDto> enteringStudentGradesRequestDtos1);
    }
}
