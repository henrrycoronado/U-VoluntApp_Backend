namespace U_VoluntApp_Core.Src.Infrastructure.Email;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var host = _configuration["SMTP_HOST"] ?? "localhost";
            var portString = _configuration["SMTP_PORT"] ?? "587";
            var username = _configuration["SMTP_USERNAME"];
            var password = _configuration["SMTP_PASSWORD"];
            var fromEmail = _configuration["SMTP_FROM_EMAIL"] ?? "no-reply@ucb.edu.bo";
            var fromName = _configuration["SMTP_FROM_NAME"] ?? "U-VoluntApp";
            var enableSslString = _configuration["SMTP_ENABLE_SSL"] ?? "true";

            if (!int.TryParse(portString, out int port))
            {
                port = 587;
            }

            if (!bool.TryParse(enableSslString, out bool enableSsl))
            {
                enableSsl = true;
            }

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = enableSsl,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                client.Credentials = new NetworkCredential(username, password);
            }

            var fromAddress = new MailAddress(fromEmail, fromName);
            var toAddress = new MailAddress(to);

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            _logger.LogInformation("Sending email to {To} via SMTP host {Host}:{Port}", to, host, port);
            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw new InvalidOperationException("No se pudo enviar el correo de verificación. Inténtalo más tarde.", ex);
        }
    }
}
