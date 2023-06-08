using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using static QRCoder.PayloadGenerator;
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;
using MimeKit;
using MailKit.Net.Imap;
using MailKit;
using Microsoft.Office.Interop.Outlook;

namespace Delta_Coop365
{
    public class Email
    {
        //private receiver;
        private string senderEmail = "deltacoop365@outlook.dk";
        private string senderPassword = "projekt1234";

        public void SendNotice(string reciever, string subject, string body, string[] attachments)
        {
            //Create a new MailMessage instance
            MailMessage mailMessage = new MailMessage(senderEmail, reciever, subject, body);
            foreach(string element in attachments)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(element));
            }
            //Call function to send the message constructed
            SendEmailViaOutlook(mailMessage);
        }

        public void SendEmailViaOutlook(MailMessage message)
        {
            try
            {

                //Create a new instance of the SmtpClient
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);


                //Send the email
                smtpClient.Send(message);
            }
            catch (System.Exception ex)
            {
                //Handle any errors that occurred during the process
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
