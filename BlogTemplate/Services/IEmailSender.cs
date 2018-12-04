using System.Threading.Tasks;

namespace Mom_Blog.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
