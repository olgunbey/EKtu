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
    public class StudentChooseLessonConfiguration : IEntityTypeConfiguration<StudentChooseLesson>
    {
        public void Configure(EntityTypeBuilder<StudentChooseLesson> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(y => y.Lesson)
                .WithMany(y => y.StudentChooseLessons)
                .HasForeignKey(y => y.LessonId);

        }
    }
}
