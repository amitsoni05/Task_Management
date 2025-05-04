using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TaskManagementSystem.Common.Utility
{
    public class EmailSender
    {
        #region Variables 
        private static readonly string SMTPHost = "smtp.office365.com";
        private static readonly int SMTPPort = 587;
        private static readonly string SMTPUserName = AppCommon.SMTP_USERNAME;
        private static readonly string SMTPPassword = AppCommon.SMTP_PASSWORD;
        #endregion

        #region Private methods

        /// <summary>
        /// Method for sending email
        /// </summary>
        /// <param name="mailMessage"></param>
        public static void SendEmail(string ToEmail, string Cc, string Name, string Subject, string Body, string Filepath = "", string bcc = "")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SmtpClient smtpClient = new SmtpClient(SMTPHost, SMTPPort);
            try
            {
                MailMessage mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    Body = Body,
                    From = new MailAddress(SMTPUserName, Name)
                };
                mailMessage.To.Add(ToEmail);
                mailMessage.Subject = Subject;

                if (!string.IsNullOrEmpty(Cc))
                {
                    mailMessage.CC.Add(Cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    mailMessage.Bcc.Add(bcc);
                }
                if (!string.IsNullOrEmpty(Filepath) && System.IO.File.Exists(Filepath))
                {
                    mailMessage.Attachments.Add(new Attachment(Filepath));
                }
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                throw;
            }
        }
       #endregion
    }
}
