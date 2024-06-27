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

        /// <summary>
        /// Displays the contact page.
        /// </summary>
        /// <returns>The contact page view.</returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Displays the privacy page.
        /// </summary>
        /// <returns>The privacy page view.</returns>
        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy Policy";
            ViewData["Message"] = "Your privacy is important to us at Acticheck. This page outlines how we collect, use, and protect your personal information.";
            return View();
        }
    }
}
