using System;
using System.Net.Mail;
using System.Net;

namespace Delta_Coop365
{
    public class EmailService
    {
        private string senderEmail = "deltacoop365@outlook.dk";
        private string senderPassword = "projekt1234";

        public void SendReport(Report report)
        {
            /// Implement code that sends the report to the receiver
        }
        public void SendNotice(string reciever, string subject, string body, string[] attachments)
        {
            MailMessage mailMessage = new MailMessage(senderEmail, reciever, subject, body);
            foreach (string element in attachments)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(element));
            }

            SendEmailViaOutlook(mailMessage);
        }

        public void SendEmailViaOutlook(MailMessage message)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
