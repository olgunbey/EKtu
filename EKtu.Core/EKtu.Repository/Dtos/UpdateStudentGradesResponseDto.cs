using EKtu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class UpdateStudentGradesResponseDto
    {
        public string FirstName{ get; set; }
        public List<ExamNoteResponseDto> ExamNoteResponseDtos{ get; set; }
    }
    public class ExamNoteResponseDto
    {
        public int Exam_1{ get; set; }
        public int Exam_2{ get; set; }
    }
}
