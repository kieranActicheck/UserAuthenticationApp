using Microsoft.AspNetCore.Mvc;
using UserAuthenticationApp.Services;

namespace UserAuthenticationApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly MsgCoordinator _msgCoordinator;

        public MessageController(MsgCoordinator msgCoordinator) {
            _msgCoordinator = msgCoordinator;
        }

        [HttpPost]
        public IActionResult ReceiveMessage([FromBody] string message)
        {
            _msgCoordinator.ProcessRequest(message);
            return Ok();
        }
    }
}
