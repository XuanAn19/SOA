using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SOA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class HelloWorldController : ControllerBase
	{
		[HttpGet]
		public IActionResult Index()
		{
			return Ok("Hello World");
		}
	}
}
