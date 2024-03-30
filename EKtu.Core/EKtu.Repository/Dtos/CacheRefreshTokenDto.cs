using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class CacheRefreshTokenDto:BaseDto
    {
        public DateTime RefreshTokenTime{ get; set; }
        public string Discriminator { get; set; }
        public string RefreshToken { get; set; }


        public override bool Equals(object? obj)
        {
            if((obj as CacheRefreshTokenDto).Id==this.Id && (obj as CacheRefreshTokenDto).Discriminator == this.Discriminator)
                return true;
            return false;
        }
    }
}
