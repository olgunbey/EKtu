using EKtu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.EntityConfigurations
{
    public class TeacherClassLessonConfiguration : IEntityTypeConfiguration<TeacherClassLesson>
    {
        public void Configure(EntityTypeBuilder<TeacherClassLesson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Lesson)
                .WithMany(x => x.TeacherClassLessons)
                .HasForeignKey(y => y.LessonId);

            builder.HasOne(y => y.Class)
                .WithMany(y => y.TeacherClassLessons)
                .HasForeignKey(y => y.ClassId);


        }
    }
}
