using EKtu.Domain.Entities;
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
    }
}
