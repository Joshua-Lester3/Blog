using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
	[HttpGet("GetBlogList")]
	public IActionResult GetBlogList()
	{
		var list = new string[] { "hi", "hiii" };
		return Ok(list);
	}
}
