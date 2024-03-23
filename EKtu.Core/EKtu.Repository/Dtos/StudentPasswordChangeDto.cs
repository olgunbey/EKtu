using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class StudentPasswordChangeDto
    {
        public int userId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string AccessToken { get; set; }
    }
}
