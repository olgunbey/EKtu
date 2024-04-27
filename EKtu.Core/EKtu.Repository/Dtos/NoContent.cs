using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class NoContent
    {
        public List<string> Errors { get; set; }
        [JsonIgnore]
        public int statusCode { get; set; }
    }
}
