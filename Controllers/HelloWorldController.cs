using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SOA.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HelloWorldController : ControllerBase
	{
		[HttpGet]
		public IActionResult Index()
		{
			return Ok("Hello World");
		}
	}
}
