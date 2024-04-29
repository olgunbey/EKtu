using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class LessonBuilder : ILessonBuilder
    {
        private Lesson _lesson;
        public LessonBuilder(Lesson lesson)
        {
            _lesson = lesson;
        }
        public Lesson GetLesson()
        {
            return _lesson;
        }

        public ILessonBuilder Grade(int grade)
        {
            _lesson.Grade = grade;
            return this;
        }

        public ILessonBuilder HasOptional(bool hasoptional)
        {
            _lesson.HasOptional = hasoptional;
            return this;
        }

        public ILessonBuilder LessonName(string lessonName)
        {
            _lesson.LessonName = lessonName;
            return this;
        }

        public ILessonBuilder OptionalLessonId(int id)
        {
            _lesson.OptionalLessonId = id;
            return this;
        }

        public ILessonBuilder Term(bool term)
        {
            _lesson.Term = term;
            return this;
        }
    }
}
