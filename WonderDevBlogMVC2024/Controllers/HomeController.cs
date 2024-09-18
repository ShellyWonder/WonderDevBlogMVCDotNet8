using System.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IBlogEmailSender _emailSender = emailSender;

        public IActionResult Index()
        {
            return View();
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
