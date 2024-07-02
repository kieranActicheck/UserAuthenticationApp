using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace UserAuthenticationApp.Pages.Account
{
    /// <summary>
    /// Page model for handling user logout functionality.
    /// </summary>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// Handles GET requests to the logout page.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to home page or a confirmation page
            return RedirectToPage("/Index");
        }
    }
}
