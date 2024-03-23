using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IRepository.StudentRepository
{
    public interface IStudentRepository:IBaseRepository<Student>
    {
        Task<IQueryable<Student>> StudentListExamGrandeAsync(int studentId);
    }
}
