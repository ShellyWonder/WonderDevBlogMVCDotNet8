using Microsoft.AspNetCore.Identity.UI.Services;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IBlogEmailSender : IEmailSender
    {
        Task SendContactEmailAsync(string emailFrom, string name, string subject, string htmlMessage);
    }
}
