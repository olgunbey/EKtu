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
    public class LessonConfirmationConfiguration : IEntityTypeConfiguration<LessonConfirmation>
    {
        public void Configure(EntityTypeBuilder<LessonConfirmation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(y => y.Lesson)
                .WithMany(y => y.LessonConfirmation)
                .HasForeignKey(y => y.LessonId);

            builder.HasOne(y => y.Student)
                .WithMany(y => y.LessonConfirmation)
                .HasForeignKey(y => y.StudentId);

            builder.HasOne(y => y.ExamNote)
                .WithOne(y => y.LessonConfirmation)
                .HasForeignKey<ExamNote>(y => y.LessonConfirmationId);
        }
    }
}
