using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.IBuilder
{
    public interface ILessonBuilder
    {
        ILessonBuilder LessonName(string lessonName);
        ILessonBuilder HasOptional(bool hasoptional);
        ILessonBuilder OptionalLessonId(int id);
        ILessonBuilder Grade(int grade);
        ILessonBuilder Term(bool term);
        Lesson GetLesson();

    }
}
