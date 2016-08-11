using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace QUERION.Common.Helpers
{
    public class EmailHelper
    {

        #region Send Email
        public static bool EmailComposeActionsAndSendEmail(string toEmailAddress, string encodedHtmlTemplateViewString, string subject, IEnumerable<string> filePaths)
        {
            try
            {
                var smtpClient = new SmtpClient();
                var message = new MailMessage();

                //smtpClient.Host = "smtp.gmail.com";
                //smtpClient.Port = 587;
                //smtpClient.Credentials = new System.Net.NetworkCredential("noesofthelpcenter@gmail.com", "TMP!3m06");
                //message.From = new MailAddress("noesofthelpcenter@gmail.com");

                message.To.Add(toEmailAddress);
                message.Subject = subject;
                message.IsBodyHtml = true;

                AlternateView messageBody = AlternateView.CreateAlternateViewFromString(encodedHtmlTemplateViewString, null, "text/html");

                if (filePaths != null)
                {
                    foreach (string filePath in filePaths)
                    {
                        var attachment = new Attachment(filePath, MediaTypeNames.Application.Octet);
                        message.Attachments.Add(attachment);
                    }
                }
                message.AlternateViews.Add(messageBody);
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
                //smtpClient.SendAsync(message, null);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion

      
    }
}
