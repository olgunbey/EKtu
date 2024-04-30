using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.IRepository.TeacherRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EKtu.Persistence.Repository.TeacherRepository
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<Teacher> EmailAndPassword(Expression<Func<Teacher, bool>> expression)
        {
          return await _dbContext.Set<Teacher>().FirstOrDefaultAsync(expression);
        }

        public Task<IQueryable<TeacherClassLesson>> TeacherClass(int teacherId)
        {
          return Task.FromResult(_dbContext.Set<TeacherClassLesson>().Where(y => y.TeacherId == teacherId).Include(y => y.Teacher).Include(y=>y.Class).Include(t=>t.Lesson).AsQueryable());
               
        }
    }
}
