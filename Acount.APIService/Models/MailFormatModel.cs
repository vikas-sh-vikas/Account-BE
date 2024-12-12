using Acount.APIService.Common;
using MailKit.Security;
using MimeKit.Text;

namespace Acount.APIService.Models
{
    public class MailFormatModelDTO
    {
        public MailFormatModel Data { get; set; }
        public ResultCodes ReturnCode { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string Description { get; set; }
    }

    public class MailFormatVO
    {
        public MailFormatModel data { get; set; }
    }
    public class MailFormatModel
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public List<string> Receivers { get; set; }
        public List<string> CCReceivers { get; set; }
        public string Subject { get; set; }
        public TextFormat Format { get; set; }
        public string MessageText { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public SecureSocketOptions SecureSocketOption { get; set; }
        public int SMTPPort { get; set; }
        public string Host { get; set; }
        public List<string> Attachments { get; set; }
    }
}
