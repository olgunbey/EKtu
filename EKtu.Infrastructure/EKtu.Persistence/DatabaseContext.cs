using EKtu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EKtu.Persistence
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions dbOptions) : base(dbOptions) { }
        public DbSet<Teacher> Teacher{ get; set; }
        public DbSet<Class> Class{ get; set; }
        public DbSet<ExamNote> ExamNote { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<OptionalLesson> OptionalLesson{ get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<TeacherClassLesson> TeacherClassLesson{ get; set; }
        public DbSet<Principal> Principal{ get; set; }
        public DbSet<StudentChooseLesson> StudentChooseLessons { get; set; }
        public DbSet<Attendance> Attendances{ get; set; }
        public DbSet<LessonConfirmation> LessonConfirmation{ get; set; }

    }
}
