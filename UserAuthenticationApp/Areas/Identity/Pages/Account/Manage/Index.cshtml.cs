using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UserAuthenticationApp.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UserAuthenticationApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<KieranProjectUser> _userManager;
        private readonly SignInManager<KieranProjectUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<KieranProjectUser> userManager,
            SignInManager<KieranProjectUser> signInManager,
            ILogger<IndexModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            Username = string.Empty;
            Email = string.Empty;
            Input = new InputModel();
            ChangePassword = new ChangePasswordModel();
            StatusMessage = string.Empty;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public ChangePasswordModel ChangePassword { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; } = string.Empty;
        }

        public class ChangePasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName ?? string.Empty;
            Email = user.Email ?? string.Empty;

            Input = new InputModel
            {
                Username = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Validation error in {Field}: {Error}", state.Key, error.ErrorMessage);
                    }
                }
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("Updating user profile for user with ID '{UserId}'.", user.Id);

            user.UserName = Input.Username;
            user.Email = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to update user profile for user with ID '{UserId}'. Errors: {Errors}", user.Id, updateResult.Errors);
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            _logger.LogInformation("Successfully updated user profile for user with ID '{UserId}'.", user.Id);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            // Ensure Username and Email are set correctly after update
            Username = user.UserName ?? string.Empty;
            Email = user.Email ?? string.Empty;

            // Redirect to the same page to refresh the data
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, ChangePassword.OldPassword, ChangePassword.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                _logger.LogError("Failed to change password for user with ID '{UserId}'. Errors: {Errors}", user.Id, changePasswordResult.Errors);
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEnableTfaAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Validation error in {Field}: {Error}", state.Key, error.ErrorMessage);
                    }
                }
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("Generating 2FA token for user with ID '{UserId}'.", user.Id);

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogError("Failed to generate 2FA token for user with ID '{UserId}'.", user.Id);
                return RedirectToPage("./Error");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogError("User with ID '{UserId}' does not have a valid email address.", user.Id);
                ModelState.AddModelError(string.Empty, "There was an error retrieving the email address. Please try again.");
                return Page();
            }

            var callbackUrl = Url.Page(
                "/Account/ConfirmTwoFactor",
                pageHandler: null,
                values: new { userId = user.Id, token = token },
                protocol: Request.Scheme);

            if (callbackUrl == null)
            {
                _logger.LogError("Failed to generate callback URL for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "There was an error generating the callback URL. Please try again.");
                return Page();
            }

            _logger.LogInformation("Sending 2FA token to user with ID '{UserId}' via email.", user.Id);

            try
            {
                await _emailSender.SendEmailAsync(
                    email,
                    "Enable Two-Factor Authentication",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                _logger.LogInformation("2FA token email sent successfully to user with ID '{UserId}'.", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending 2FA token email to user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "There was an error sending the email. Please try again.");
                return Page();
            }

            return RedirectToPage();
        }


    }
}
