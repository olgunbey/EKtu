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
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(y => y.StudentChooseLesson)
                .WithMany(y => y.Attendance)
                .HasForeignKey(y => y.StudentChooseLessonId);

            builder.Property(y => y.PermissionCheck).HasDefaultValue(false);
        }
    }
}
