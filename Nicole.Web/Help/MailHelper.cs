using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Nicole.Web.Models;

namespace Nicole.Web.Help
{
    public static class MailHelper
    {
        public static bool SendRegisterMail(RegisterViewModel model)
        {
            var username = ConfigurationManager.AppSettings["MailUser"];
            var pwd = ConfigurationManager.AppSettings["MailPWD"];
            var host = ConfigurationManager.AppSettings["Mailhost"];
            var url = "http://" + HttpContext.Current.Request.Url.Host + "/Account/ConfirmEmail?VerificationCode=" + model.VerificationCode;
            return SendMail(username, pwd, model.Email, host, "Confirmation of registration", url, string.Empty);
        }
        public static bool SendMail(string userName, string pwd, string mailaddress, string host, string sub, string body, string filePath)
        {
            var client = new SmtpClient
            {
                Host = host,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, pwd),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            try
            {
                var message = new MailMessage(userName, mailaddress)
                {
                    Subject = sub,
                    Body = body,
                    BodyEncoding = System.Text.Encoding.UTF8,
                    IsBodyHtml = true,
                };
                if (!string.IsNullOrEmpty(filePath))
                {
                    var attachment = new Attachment(filePath) { Name = filePath.Split('/').LastOrDefault() };
                    message.Attachments.Add(attachment);
                }
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}