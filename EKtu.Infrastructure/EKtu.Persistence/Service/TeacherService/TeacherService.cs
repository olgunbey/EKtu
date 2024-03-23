using EKtu.Domain.Entities;
using EKtu.Repository.IRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.TeacherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.TeacherService
{
    public class TeacherService : BaseService<Teacher>, ITeacherService
    {
        public TeacherService(IBaseRepository<Teacher> baseRepository, ISaves saves) : base(baseRepository, saves)
        {
        }
    }
}
