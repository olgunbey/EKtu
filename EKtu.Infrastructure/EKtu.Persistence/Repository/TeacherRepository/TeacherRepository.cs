using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.IRepository.TeacherRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Repository.TeacherRepository
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}
