using Microsoft.AspNetCore.Mvc;

namespace UserAuthenticationApp.Controllers
{
    /// <summary>
    /// Controller responsible for handling non-authentication related actions.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the home page.
        /// </summary>
        /// <returns>The home page view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Displays the about page.
        /// </summary>
        /// <returns>The about page view.</returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
