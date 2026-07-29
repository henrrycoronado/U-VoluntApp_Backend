namespace U_VoluntApp_Core.Src.Infrastructure.Email;

using System.Threading.Tasks;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
