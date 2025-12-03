using Core.Interfaces.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class SmtpEmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
     

        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
          
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Leemos la config
                var settings = _configuration.GetSection("SmtpSettings");
                var server = settings["Server"];
                var port = int.Parse(settings["Port"]);
                var senderEmail = settings["SenderEmail"];
                var senderName = settings["SenderName"];
                var username = settings["Username"];
                var password = settings["Password"]; // La App Password de Google


                using (var client = new SmtpClient(server, port))
                {
                    client.EnableSsl = true;

                 
                    client.UseDefaultCredentials = false;

                    
                    client.Credentials = new NetworkCredential(username, password);  ;

                    // Preparamos el mensaje
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true 
                    };

                    mailMessage.To.Add(toEmail);

                  
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine($"Email enviado correctamente a {toEmail}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR CRÍTICO AL ENVIAR EMAIL: {ex.Message}");
                // Para desarrollo, es útil ver la excepción interna
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}


