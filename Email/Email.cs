using System.Net.Mail;
using System.Net;

namespace MVCFinalProject.Email
{
    public class CreateEmail
    {
        public bool SendEmailWithPDF(string recipientEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient("smtp.ethereal.email"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("federico.langworth70@ethereal.email", "jkkHjT66hXY4awwvN3");
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress("federico.langworth70@ethereal.email");
                        mailMessage.To.Add(recipientEmail);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;

                        // Attach the PDF file
                        if (!string.IsNullOrEmpty(attachmentPath) && System.IO.File.Exists(attachmentPath))
                        {
                            Attachment attachment = new Attachment(attachmentPath);
                            mailMessage.Attachments.Add(attachment);
                        }
                        else
                        {
                            throw new Exception("Attachment file not found.");
                        }

                        smtpClient.Send(mailMessage);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                return false;
            }
        }
    }
}

