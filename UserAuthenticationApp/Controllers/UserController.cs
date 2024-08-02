using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UserAuthenticationApp.Data;
using UserAuthenticationApp.DTOs;

namespace UserAuthenticationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<KieranProjectUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<KieranProjectUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            _logger.LogInformation("GetUserById called with id: {Id}", id);
            id = id.Trim(); // Trim any leading or trailing whitespace
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with id: {Id}", id);
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Forename = user.Forename,
                Surname = user.Surname,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate
            };

            _logger.LogInformation("User found with id: {Id}, UserName: {UserName}, Email: {Email}", id, user.UserName, user.Email);
            return Ok(userDto);
        }
    }
}
