using DinkToPdf;
using DinkToPdf.Contracts;
using EKtu.Repository.Dtos;
using EKtu.Repository.IService.PdfService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.PdfServices
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;
        public PdfService(IConverter converter)
        {
            _converter=converter;
        }
        public byte[] PdfBytes(StudentCertificateResponseDto studentCertificateResponseDto)
        {
            string tcKimlikNo = "1234567890";
            string adSoyad = "Olgun Bey Şahin";
            string bolum = "Yazılım";
            string okul = "Ktü";

            string htmlContent = String.Format(@"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Örnek PDF</title>
</head>
<body>

    <div style=""text-align: center; font-size: 20px;"">
        <p><strong> T.C  </strong></p>
        <p><strong>  YÜKSEKOĞRETİM KURULU BAŞKANLIĞI </strong></p>
        <p><strong>   ANKARA </strong></p>

    </div>

    <div style=""display: flex; flex-direction: column; justify-content: baseline; min-width: 100%; margin-left: 22%; padding: 2rem;"">
    <div class=""""profil-img"""" style=""margin-bottom:10px; padding: 2rem 0 2rem 0;"">
        <img src=""kedii.jpg"" alt=""Profil Resmi"">
    </div>
    <div style=""font-size:20px; margin-bottom:50px; text-align:left; "">
        <p><strong>Tc Kimlik No:</strong> {0}</p>
        <p><strong>Adı Soyadı:</strong> {1}</p>
        <p><strong>Okuduğu Bölüm:</strong> {2}</p>
        <p><strong>Okuduğu Okul:</strong> {3}</p>
        <p><strong>Sinif </strong> {4}</p>
        <p><strong>Kayit tarihi </strong> {5}</p>
        <p><strong>Öğrencilik Durumu </strong> {6}</p>
    </div>
</div>
    <div class=""""lorem"""" style=""text-align:center;"">
        <strong>ILGILI MAKAMA </strong>
        <p>Yukarıda kimlik bilgileri yer alan {1} isimli kişinin Karadeniz Teknik Üniversitesi tarafından yukarıda belirtilen
programın kayıtlı öğrencisi olduğu bildirilmiştir.
</p>
    </div>
</body>
</html>", tcKimlikNo, adSoyad, bolum, okul);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    ColorMode=ColorMode.Color,
                    Orientation=Orientation.Landscape,
                    PaperSize=PaperKind.A4Plus
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        PagesCount=true,
                        HtmlContent=htmlContent,
                        WebSettings={DefaultEncoding="utf-8"}
                    }
                }
            };
            return _converter.Convert(doc);
        }
    }
}
