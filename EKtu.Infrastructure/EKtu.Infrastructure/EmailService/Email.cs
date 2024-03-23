using EKtu.Repository.Dtos;
using EKtu.Repository.IService.EmailService;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKtu.Infrastructure.EmailService
{
    public class Email : IEmail
    {
        private readonly IOptions<Configuration> options;
        public Email(IOptions<Configuration> options)
        {
            this.options = options;
        }
        public async Task<Response<bool>> SendMail(string targetMail,string address)
        {
            try
            {
                var email = new MimeMessage();
                email.Subject = "Ektu şifre sıfırlama";
                email.From.Add(MailboxAddress.Parse(options.Value.mail));
                email.To.Add(MailboxAddress.Parse(targetMail));

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<a href={address}> buradaki linke tıklayın </a>",

                };

                using (SmtpClient smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(options.Value.mail, options.Value.mailPassword);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);

                    return Response<bool>.Success(true, 200);
                }
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail("hata mail gönderilmedi", 400);
            }
            
        }
    }
}
