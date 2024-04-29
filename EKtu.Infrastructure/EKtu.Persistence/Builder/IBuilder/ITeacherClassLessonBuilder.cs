using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface ITeacherClassLessonBuilder
    {
        ITeacherClassLessonBuilder TeacherId(int teacherId);
        ITeacherClassLessonBuilder ClassId(int classId);
        ITeacherClassLessonBuilder LessonId(int lessonId);
        TeacherClassLesson Get();
    }
}
