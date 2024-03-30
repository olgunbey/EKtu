using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class GenerateTokenResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenLifeTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenLifeTime { get; set; }
    }
}
