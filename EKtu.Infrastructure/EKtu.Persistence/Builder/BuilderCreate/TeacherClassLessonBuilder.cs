using EKtu.Domain.Entities;
using EKtu.Persistence.Builder.IBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Persistence.Builder.BuilderCreate
{
    public class TeacherClassLessonBuilder : ITeacherClassLessonBuilder
    {
        private readonly TeacherClassLesson _teacherClassLesson;
        public TeacherClassLessonBuilder(TeacherClassLesson teacherClassLesson)
        {
            _teacherClassLesson = teacherClassLesson;
        }
        public ITeacherClassLessonBuilder ClassId(int classId)
        {
            _teacherClassLesson.ClassId = classId;
            return  this;
        }

        public TeacherClassLesson Get()
        {
            return _teacherClassLesson;
        }

        public ITeacherClassLessonBuilder LessonId(int lessonId)
        {
            _teacherClassLesson.LessonId = lessonId;
            return this;
        }

        public ITeacherClassLessonBuilder TeacherId(int teacherId)
        {
            _teacherClassLesson.TeacherId = teacherId;
            return this;
        }
    }
}
