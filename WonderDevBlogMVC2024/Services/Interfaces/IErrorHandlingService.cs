using Microsoft.AspNetCore.Mvc;
using WonderDevBlogMVC2024.Areas.Identity.Pages;

namespace WonderDevBlogMVC2024.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        ViewResult HandleError(string errorMessage);
    }
}
