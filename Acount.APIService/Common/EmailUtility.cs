using Acount.APIService.Models;
using MimeKit.Utils;
using MimeKit;
using MailKit.Net.Smtp;

namespace Acount.APIService.Common
{
    public class EmailUtility
    {
        public static bool SendMail(MailFormatModel mailFormat)
        {
            //Initialize Smtp Credential
            SmtpClient smtp = new SmtpClient();
            try
            {

                MimeMessage emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(mailFormat.SenderName, mailFormat.SenderEmail));
                emailMessage.To.Add(new MailboxAddress(mailFormat.Receivers[0], mailFormat.Receivers[0]));
                //emailMessage.To.AddRange(mailFormat.Receivers.Select(x => new MailboxAddress(x.ToString(),)));
                emailMessage.Subject = mailFormat.Subject;
                //mailFormat.Format = MimeKit.Text.TextFormat.Html;
                //emailMessage.Body = new TextPart(mailFormat.Format) { Text = mailFormat.MessageText };

                //To add image in email body
                var builder = new BodyBuilder { HtmlBody = mailFormat.MessageText };
                var image = builder.LinkedResources.Add(@EmailUtility.EmailLogo());
                image.ContentId = MimeUtils.GenerateMessageId();
                mailFormat.MessageText = mailFormat.MessageText.Replace("ContentId", image.ContentId);
                builder.HtmlBody = mailFormat.MessageText;

                emailMessage.Body = builder.ToMessageBody();

                smtp.Connect(mailFormat.Host, mailFormat.SMTPPort, mailFormat.SecureSocketOption);
                smtp.Authenticate(mailFormat.UserName, mailFormat.Password);
                smtp.Send(emailMessage);
                smtp.Disconnect(true);
                emailMessage.Dispose();

            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

    


        public static string WelcomeMail()
        {
            string filename = Path.Combine(Environment.CurrentDirectory, Constants.TemplateFolder, Constants.WelcomeTemplate);
            return File.ReadAllText(filename);
        }

        public static string EmailLogo()
        {
            return Path.Combine(Environment.CurrentDirectory, Constants.TemplateFolder, Constants.EmailLogo);
        }

    }

}
