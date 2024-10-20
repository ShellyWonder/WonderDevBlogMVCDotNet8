using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WonderDevBlogMVC2024.Areas.Identity.Pages;
using WonderDevBlogMVC2024.Services.Interfaces;

namespace WonderDevBlogMVC2024.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        #region HANDLE ERROR
        public ViewResult HandleError(string errorMessage)
        {
            var errorModel = new ErrorModel
            {
                CustomErrorMessage = errorMessage
            };

            // Manually construct and return a ViewResult with the error model
            return new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<ErrorModel>(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = errorModel
                }
            };
        } 
        #endregion
    }
}
