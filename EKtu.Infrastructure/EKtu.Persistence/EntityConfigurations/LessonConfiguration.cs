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
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(y => y.Id);

            builder.Property(y => y.OptionalLessonId).IsRequired(false);

            builder.HasOne(y => y.OptionalLesson)
                .WithMany(y => y.Lessons)
                .HasForeignKey(y => y.OptionalLessonId);
        }
    }
}
