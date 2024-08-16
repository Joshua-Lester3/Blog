using Blog.Api.Dtos;
using Blog.Api.Identity;
using Blog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
	private readonly BlogService _service;

	public BlogController(BlogService service)
	{
		_service = service;
	}

	[HttpGet("GetBlogList")]
	public async Task<IActionResult> GetBlogList()
	{
		var blogList = await _service.GetBlogList();

		return Ok(blogList);
	}

	[HttpPost("PostBlogPost")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> AddBlogPost(BlogPostDto dto)
	{
		if (dto.Title is null)
		{
			return BadRequest("Title cannot be null.");
		}
		if (dto.Content is null)
		{
			return BadRequest("Content cannot be null.");
		}
		if (dto.Title.Trim().Length == 0)
		{
			return BadRequest("Title cannot contain only whitespace or be empty.");
		}
		if (dto.Content.Trim().Length == 0)
		{
			return BadRequest("Content cannot contain only whitespace or be empty.");
		}

		var blogPost = await _service.PostBlogPost(dto);
		return Ok(blogPost);
	}

	[HttpGet("GetBlogPost")]
	public async Task<IActionResult> GetBlogPost(int id)
	{
		var result = await _service.GetBlogPost(id);

		if (result is null)
		{
			return BadRequest();
		}
		return Ok(result);
	}

	[HttpPost("DeleteBlogPost/{id}")]
	[Authorize(Roles = Roles.Admin)]
	public async Task<IActionResult> DeleteBlogPost([FromRoute] int id)
	{
		var result = await _service.DeleteBlogPost(id);

		if (result)
		{
			return Ok();
		}
		return BadRequest("Something went wrong while deleting post.");
	}
}
