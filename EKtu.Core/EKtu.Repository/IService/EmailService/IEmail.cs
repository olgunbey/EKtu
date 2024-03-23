using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.EmailService
{
    public interface IEmail
    {
        Task<Response<bool>> SendMail(string targetMail,string address); 
    }
}
