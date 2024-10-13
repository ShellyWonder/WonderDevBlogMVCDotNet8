using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WonderDevBlogMVC2024.Enums;
using WonderDevBlogMVC2024.Models;
using WonderDevBlogMVC2024.Services.Interfaces;
using WonderDevBlogMVC2024.ViewModels;

namespace WonderDevBlogMVC2024.Controllers
{
    #region PRIMARY CONSTRUCTOR
    public class HomeController(ILogger<HomeController> logger, 
                                IBlogEmailSender emailSender,
                                IBlogService blogService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IBlogEmailSender _emailSender = emailSender;
        private readonly IBlogService _blogService = blogService;
        #endregion

        #region GET INDEX/productionReady posts by blog
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;
            //Grabs all status: productionReady posts by blog in descending order by date
            var blogs = await _blogService.GetBlogsByStateAsync(PostState.ProductionReady, pageNumber, pageSize);
            return View(blogs);
        }
        #endregion

        #region GET ABOUT
        public IActionResult About()
        {
            return View();
        }
        #endregion

        #region GET CONTACT
        public IActionResult Contact()
        {
            return View();
        }
        #endregion

        #region POST CONTACT
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
        #endregion

        #region ERROR
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
