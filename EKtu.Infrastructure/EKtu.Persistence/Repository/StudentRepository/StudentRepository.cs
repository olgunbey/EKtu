using EKtu.Domain.Entities;
using EKtu.Persistence.Repositorys;
using EKtu.Repository.IRepository.StudentRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Repository.StudentRepository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public Task<IQueryable<Student>> StudentListExamGrandeAsync(int studentId)
        {
          return Task.FromResult(_dbContext.Student.Where(y=>y.Id==studentId).Include(y => y.StudentChooseLessons)
                .ThenInclude(y => y.ExamNote)
                .Include(y => y.StudentChooseLessons)
                .ThenInclude(y => y.Lesson).AsQueryable());
        }
    }
}
