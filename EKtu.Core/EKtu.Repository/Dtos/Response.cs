using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EKtu.Repository.Dtos
{
    public class Response<T>
    {
        [JsonIgnore]
        public int StatusCode{ get; set; }
        public T Data{ get; set; }
        public List<string> Errors { get; set; }
        public bool IsSuccessfull{ get; set; }
        public static Response<T> Success(T data,int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, Data = data, IsSuccessfull=true };
        }
        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, IsSuccessfull = true };
        }
        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, Errors = errors, IsSuccessfull = false };
        }
        public static Response<T> Fail(string errors,int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { errors}, IsSuccessfull = false,StatusCode=statusCode };
        }
    }
}
