using System;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyBanHang.Services
{
    public class GmailServices
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _email;
        private readonly string _password;

        public GmailServices(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new System.Net.NetworkCredential(_email, _password);
                client.EnableSsl = true;

                var mailMessage = new MailMessage(_email, toEmail, subject, body);
                mailMessage.IsBodyHtml = true;

                client.Send(mailMessage);
            }
        }
    }
}
