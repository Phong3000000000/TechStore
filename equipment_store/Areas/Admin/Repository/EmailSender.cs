using System.Net.Mail;
using System.Net;

namespace equipment_store.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("phongporo.com@gmail.com", "rsdndfropvywoqyw")
            };

            return client.SendMailAsync(
                new MailMessage(from: "phongporo.com@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
