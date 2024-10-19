using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Service.Services.User;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController(IUserService userService) : BaseController
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> SendFriendRequest([FromQuery] string username)
        {
            return HandleServiceResult(await _userService.SendFriendRequest(username));
        }

        [HttpGet]
        public async Task<IActionResult> GetReceivedFriendRequests()
        {
            return HandleServiceResult(await _userService.GetReceivedFriendRequests());
        }

        [HttpGet]
        public async Task<IActionResult> GetSentFriendRequests()
        {
            return HandleServiceResult(await _userService.GetSentFriendRequests());
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            return HandleServiceResult(await _userService.GetFriends());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFriendRequest(int friendRelationId)
        {
            return HandleServiceResult(await _userService.DeleteFriendRequest(friendRelationId));
        }

        [HttpPost]
        public async Task<IActionResult> AcceptFriendResult([FromQuery] int friendRelationId)
        {
            return HandleServiceResult(await _userService.AcceptFriendRequest(friendRelationId));
        }

        [HttpPost]
        public async Task<IActionResult> DeclineFriendRequest([FromQuery] int frienRelationId)
        {
            return HandleServiceResult(await _userService.DeclineFriendRequest(frienRelationId));
        }
    }
}
