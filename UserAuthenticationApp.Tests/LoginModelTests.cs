using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAuthenticationApp.Areas.Identity.Pages.Account;
using UserAuthenticationApp.Data;
using Xunit;

namespace UserAuthenticationApp.Tests
{
    /// <summary>
    /// Contains unit tests for the LoginModel class.
    /// </summary>
    public class LoginModelTests
    {
        /// <summary>
        /// Creates a mock UserManager for KieranProjectUser.
        /// </summary>
        /// <returns>A mock UserManager instance.</returns>
        private Mock<UserManager<KieranProjectUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<KieranProjectUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<KieranProjectUser>>();
            var userValidators = new List<IUserValidator<KieranProjectUser>> { new Mock<IUserValidator<KieranProjectUser>>().Object };
            var passwordValidators = new List<IPasswordValidator<KieranProjectUser>> { new Mock<IPasswordValidator<KieranProjectUser>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<KieranProjectUser>>>();

            return new Mock<UserManager<KieranProjectUser>>(store.Object, options.Object, passwordHasher.Object, userValidators, passwordValidators, keyNormalizer.Object, errors.Object, services.Object, logger.Object);
        }

        /// <summary>
        /// Creates a mock SignInManager for KieranProjectUser.
        /// </summary>
        /// <param name="mockUserManager">The mock UserManager instance.</param>
        /// <returns>A mock SignInManager instance.</returns>
        private Mock<SignInManager<KieranProjectUser>> GetMockSignInManager(Mock<UserManager<KieranProjectUser>> mockUserManager)
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<KieranProjectUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<KieranProjectUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<KieranProjectUser>>();

            return new Mock<SignInManager<KieranProjectUser>>(
                mockUserManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                options.Object,
                logger.Object,
                schemes.Object,
                confirmation.Object);
        }

        /// <summary>
        /// Tests that OnPostAsync redirects to the return URL when valid credentials are provided.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_ValidCredentials_RedirectsToReturnUrl()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockSignInManager = GetMockSignInManager(mockUserManager);
            var mockLogger = new Mock<ILogger<LoginModel>>();

            var loginModel = new LoginModel(mockSignInManager.Object, mockLogger.Object)
            {
                Input = new LoginModel.InputModel
                {
                    Email = "valid@example.com",
                    Password = "ValidPassword123",
                    RememberMe = false
                }
            };

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await loginModel.OnPostAsync("/Home");

            // Assert
            var redirectToPageResult = Assert.IsType<LocalRedirectResult>(result);
            Assert.Equal("/Home", redirectToPageResult.Url);
        }

        /// <summary>
        /// Tests that OnPostAsync returns the page with an error when invalid credentials are provided.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_InvalidCredentials_ReturnsPageWithError()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockSignInManager = GetMockSignInManager(mockUserManager);
            var mockLogger = new Mock<ILogger<LoginModel>>();

            var loginModel = new LoginModel(mockSignInManager.Object, mockLogger.Object)
            {
                Input = new LoginModel.InputModel
                {
                    Email = "invalid@example.com",
                    Password = "InvalidPassword123",
                    RememberMe = false
                }
            };

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await loginModel.OnPostAsync("/Home");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.True(loginModel.ModelState.ContainsKey(string.Empty));
            var errorMessage = loginModel.ModelState[string.Empty]?.Errors[0]?.ErrorMessage;
            Assert.Equal("Invalid login attempt.", errorMessage);
        }

        /// <summary>
        /// Tests that OnPostAsync redirects to the lockout page when the user is locked out.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_UserLockedOut_RedirectsToLockoutPage()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockSignInManager = GetMockSignInManager(mockUserManager);
            var mockLogger = new Mock<ILogger<LoginModel>>();

            var loginModel = new LoginModel(mockSignInManager.Object, mockLogger.Object)
            {
                Input = new LoginModel.InputModel
                {
                    Email = "lockedout@example.com",
                    Password = "LockedOutPassword123",
                    RememberMe = false
                }
            };

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            // Act
            var result = await loginModel.OnPostAsync("/Home");

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./Lockout", redirectToPageResult.PageName);
        }

        /// <summary>
        /// Tests that OnPostAsync redirects to the two-factor authentication page when two-factor authentication is required.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_TwoFactorRequired_RedirectsToTwoFactorPage()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockSignInManager = GetMockSignInManager(mockUserManager);
            var mockLogger = new Mock<ILogger<LoginModel>>();

            var loginModel = new LoginModel(mockSignInManager.Object, mockLogger.Object)
            {
                Input = new LoginModel.InputModel
                {
                    Email = "2fa@example.com",
                    Password = "2faPassword123",
                    RememberMe = false
                }
            };

            mockSignInManager
                .Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

            // Act
            var result = await loginModel.OnPostAsync("/Home");

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("./LoginWith2fa", redirectToPageResult.PageName);
        }

        /// <summary>
        /// Tests that OnPostAsync returns the page with errors when the model state is invalid.
        /// </summary>
        [Fact]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageWithErrors()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockSignInManager = GetMockSignInManager(mockUserManager);
            var mockLogger = new Mock<ILogger<LoginModel>>();

            var loginModel = new LoginModel(mockSignInManager.Object, mockLogger.Object);
            loginModel.ModelState.AddModelError("Input.Email", "The Email field is required.");

            // Act
            var result = await loginModel.OnPostAsync("/Home");

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.False(loginModel.ModelState.IsValid);
        }
    }
}

