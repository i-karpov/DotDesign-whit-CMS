using System;
using System.Net;
using System.Web.Mvc;

namespace DotDesign.WebUI.Controllers
{
    /// <summary>
    /// Handles "after-error" redirections.
    /// </summary>
    public class ErrorsController : Controller
    {
        /// <summary>
        /// Represents Bad Request HTTP status code.
        /// </summary>
        private const int BadRequestStatusCode = 400;

        /// <summary>
        /// Sets response status to 404 and returns default 404-error page.
        /// </summary>
        public ViewResult HttpError404()
        {
            Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotFound);
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}
