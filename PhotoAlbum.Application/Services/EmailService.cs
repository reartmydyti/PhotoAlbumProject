using PhotoAlbum.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhotoAlbum.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _smtpSettings;

        public EmailService(EmailSettings smptSettings)
        {
            _smtpSettings = smptSettings;
        }

        private async Task SendEmailAsync(string fromMail, string toMail, string subject, string htmlMessage, string replyTo)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.To.Add(new MailAddress(toMail));
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;

            // Add the reply-to address
            if (!string.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo));
            }

            try
            {
                using (var smtpClient = new SmtpClient(_smtpSettings.Smtp_Server))
                {
                    smtpClient.Port = _smtpSettings.SmtpPort_STARTTLS; 
                    smtpClient.Credentials = new NetworkCredential(fromMail, _smtpSettings.Password);
                    smtpClient.EnableSsl = _smtpSettings.UseSsl;

                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
                Console.WriteLine($"StatusCode: {smtpEx.StatusCode}");
                if (smtpEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {smtpEx.InnerException.Message}");
                }
                throw;
            }
            catch (SocketException socketEx)
            {
                Console.WriteLine($"Socket Exception: {socketEx.Message}");
                Console.WriteLine($"Socket Error Code: {socketEx.SocketErrorCode}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                throw;
            }
        



        //public async Task SendEmailAsync_NoReply(string toEmail, string subject, string htmlMessage)
        //{
        //    await SendEmailAsync(_smtpSettings.Username_NoReply, toEmail, subject, htmlMessage);
        //}

        //public async Task SendEmailAsync_Support(string toEmail, string subject, string htmlMessage)
        //{
        //    await SendEmailAsync(_smtpSettings.Username_Support, toEmail, subject, htmlMessage);
        //}

        //public async Task SendEmailAsync_Feedback(string toEmail, string subject, string htmlMessage)
        //{
        //    await SendEmailAsync(_smtpSettings.Username_Feedback, toEmail, subject, htmlMessage);
        }


        public async Task SendContactEmailAsync(string fromEmail, string firstName, string lastName, string description)
                    {
                        string toEmail = _smtpSettings.Username_NoReply; // Your recipient email
                        string subject = "Contact Form Submission";
                        string htmlMessage = $@"
                    <h2>Contact Form Submission</h2>
                    <p><strong>First Name:</strong> {firstName}</p>
                    <p><strong>Last Name:</strong> {lastName}</p>
                    <p><strong>Email:</strong> {fromEmail}</p>
                    <p><strong>Description:</strong></p>
                    <p>{description}</p>
                ";

                        // Use the SMTP settings username as the sender
                        await SendEmailAsync(_smtpSettings.Username_NoReply, toEmail, subject, htmlMessage, fromEmail);
                    }

    }




    public class EmailSettings
    {
        public string Username_NoReply { get; set; }
        public string Username_Support { get; set; }
        public string Username_Feedback { get; set; }
        public string Smtp_Server { get; set; }
        public int SmtpPort_SSL_TLS { get; set; }
        public int SmtpPort_STARTTLS { get; set; }
        public int SmtpPort_STARTTLS2 { get; set; }
        public string IMAP_Server { get; set; }
        public int IMAP_SSL_TLS { get; set; }
        public int IMAP_STARTTLS { get; set; }
        public string POP3_Server { get; set; }
        public int POP3_SSL_TLS { get; set; }
        public int POP3_STARTTLS { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
    }
}
