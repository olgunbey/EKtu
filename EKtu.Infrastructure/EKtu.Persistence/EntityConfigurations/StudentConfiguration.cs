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
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(y => y.Id);

            builder.Property(y=>y.TckNo).HasMaxLength(11);

            builder.HasOne(y => y.Class)
                .WithMany(y => y.Students)
                .HasForeignKey(y => y.ClassId);

            builder.HasMany(y => y.StudentChooseLessons)
                .WithOne(y => y.Student)
                .HasForeignKey(y => y.StudentId);
        }
    }
}
