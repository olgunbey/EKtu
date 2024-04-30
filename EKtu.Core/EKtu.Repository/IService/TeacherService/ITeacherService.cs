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
        Task TeacherLesson(int classId,int teacherId);


    }
}
