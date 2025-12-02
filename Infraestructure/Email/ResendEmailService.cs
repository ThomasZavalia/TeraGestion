using Core.Interfaces.Email;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class ResendEmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ResendEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.resend.com");

            var apiKey = _configuration["Resend:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromEmail = _configuration["Resend:FromEmail"];

            var emailData = new
            {
                from = $"TeraGestion <{fromEmail}>",
                to = new[] { toEmail },
                subject = subject,
                html = body
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/emails", emailData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Email enviado a {toEmail}");
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error enviando email Resend: {error}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción enviando email: {ex.Message}");
                return false;
            }
        }
    }
}

