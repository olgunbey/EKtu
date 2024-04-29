using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository.PrincipalRepository
{
    public interface IPrincipalRepository:IBaseRepository<Principal>
    {
        Task StudentLessonApproveAsync();
        Task AddLessonsAsync(Lesson lesson);
    }
}
