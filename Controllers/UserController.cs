using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOA.Data;
using SOA.DTOs;
using SOA.Service.Interface;

namespace SOA.Controllers
{
	[Route("api")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		[HttpPost("/register")]
		public async Task<IActionResult> Register([FromBody] UserDTO user)
		{
			try
			{
				if (user != null)
				{
					await _userService.RegisterUser(user);
					return Ok(new {status = "ok", message = "Đăng kí thành công"});
				}
				else
				{
					return BadRequest(new { status = "no", message = "Dữ liệu không có" });
				}

			}
			catch (Exception ex)
			{
				return BadRequest(new { status = "no", message = ex.Message });
			}
		}
		[HttpPost("/auth")]
		public async Task<IActionResult> Login([FromBody] UserDTO userLogin)
		{
			var login = await _userService.UserLogin(userLogin.Email, userLogin.Password);
			return Ok(login);
		}
	}
}
