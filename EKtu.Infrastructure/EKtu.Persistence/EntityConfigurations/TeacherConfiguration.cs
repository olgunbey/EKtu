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
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(y => y.Id);

            builder.Property(y=>y.Password).HasMaxLength(11);

            builder.HasMany(y => y.TeacherClassLessons)
                .WithOne(y => y.Teacher)
                .HasForeignKey(y => y.TeacherId);
        }
    }
}
