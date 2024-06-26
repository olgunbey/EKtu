﻿using EKtu.Domain.Entities;
using EKtu.Repository.Dtos;
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
        Task StudentChooseLessonAsync(StudentChooseLessonRequestDto studentChooseLessonRequestDto,int studentId);


        Task<Student> StudentCertificateAsync(int userId);
        Task<List<StudentAbsenceDto>> SelectingStudentAbsent(int userId);

        Task<IQueryable<StudentChooseLesson>> GetStudentChooseLessonAsync(int userId);
        Task<int> StudentChangeLessonAsync(List<StudentChangeLessonRequestDto> studentChangeLessonRequestDtos, int studentId);

        Task<IQueryable<StudentChooseLesson>> AllStudentChooseLessonAsync();

        Task<IQueryable<LessonConfirmation>> AllStudentExamGrande();

        Task<IQueryable<Class>> ClassAllStudentExamGrandeList();

        Task<IQueryable<Class>> GetClassList();

        ValueTask<Student> GetStudentClassIdWithStudentIdAsync(int studentId);
        Task<Student> StudentInformation(int userId);
        Task<IQueryable<Class>> GetAllClassList();
        Task<IQueryable<Lesson>> GetLessonTerm(bool termLessonListRequestDto, int Grade);
    }
}
