using EKtu.Repository.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Repository.IService.PdfService
{
    public interface IPdfService
    {
        public byte[] PdfBytes(StudentCertificateResponseDto studentCertificateResponseDto); 
    }
}
