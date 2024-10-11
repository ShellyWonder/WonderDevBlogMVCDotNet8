using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;
using X.PagedList;

namespace WonderDevBlogMVC2024.Controllers
{
    public class HomeController(ILogger<HomeController> logger, 
                                IBlogEmailSender emailSender,
                                IBlogService blogService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IBlogEmailSender _emailSender = emailSender;
        private readonly IBlogService _blogService = blogService;

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var blogs = await _blogService.GetAllProdBlogsAsync(pageNumber, pageSize);
            return View(blogs);
        }

         public IActionResult About()
        {
            return View();
        }
        //Get
         public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactUs model)
        {
            //build email
            model.Message = $"{model.Message}<hr>" +
            //using the Ternary (Conditional) operator (??) to check whether model.Phone is null 
            $"Phone: {(string.IsNullOrWhiteSpace(model.Phone) ? "Phone: Not Provided" : model.Phone)}";
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return RedirectToAction("Index");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
