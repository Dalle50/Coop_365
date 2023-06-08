using System;
using System.Net.Mail;
using System.Net;


namespace Delta_Coop365
{
    /// <summary>
    /// [Author] Daniel
    /// </summary>
    public class Email
    {
        //testemails setup for this cause only
        private string senderEmail = "deltacoop365@outlook.dk";
        private string senderPassword = "projekt1234";

        /// <summary>
        /// Funktion der konstruerer en email, som kalder en anden funktion til at sende den.
        /// </summary>
        /// <param name="reciever"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
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
        /// <summary>
        /// Sender mailen som er vedhæftet med hardkodede sender og password, da det er en butiksmail.
        /// </summary>
        /// <param name="message"></param>
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
                message.Dispose();
                smtpClient.Dispose();
            }
            catch (System.Exception ex)
            {
                //Handle any errors that occurred during the process
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}
