using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace BAL.Utils
{
    public class FunctionSendMail
    {
        public static async Task SendEmail(string mailTo, string subject, string bodyEmail)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse("hodangkhoa110701@gmail.com"));
            email.To.Add(MailboxAddress.Parse(mailTo));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = bodyEmail };

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync("smtp.gmail.com", 465, true);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync("hodangkhoa110701@gmail.com", "geiloooziywbqrcn");

                await smtp.SendAsync(email);
            }
            catch
            {
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
