using Microsoft.AspNetCore.Mvc;
using TicTacToe.Domain;
using TicTacToe.WebApi.Models.Dto;

namespace TicTacToe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
        {
            try
            {
                var user = new Player
                {
                    Username = request.Username,
                    Password = request.Password,
                    Score = 0,
                    GameResults = new List<GameResult>(),
                };

                await _userService.Register(user);

                return Ok("User registered success");
            }
            catch (Exception)
            {
                return StatusCode(500, "User already exists");
            }
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            try
            {
                var user = await _userService.Authenticate(request.Username, request.Password);

                if (user == null)
                    return Unauthorized("Invalid username or password");

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to sign in");
            }
        }
    }
}
