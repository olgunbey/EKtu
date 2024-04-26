using EKtu.Domain.Entities;
using EKtu.Infrastructure.HASH;
using EKtu.Repository.IRepository;
using EKtu.Repository.IService;
using EKtu.Repository.IService.TeacherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Service.TeacherService
{
    public class TeacherService : BaseService<Teacher>, ITeacherService
    {
        private readonly IPasswordRepository<Teacher> passwordRepository;
        public TeacherService(IBaseRepository<Teacher> baseRepository, ISaves saves, IPasswordRepository<Teacher> passwordRepository) : base(baseRepository, saves)
        {
            this.passwordRepository = passwordRepository;
        }
    }
}
