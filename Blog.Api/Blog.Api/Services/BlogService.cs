using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api.Services;

public class BlogService
{
	private readonly AppDbContext _context;

	public BlogService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<BlogPost>> GetBlogList()
	{
		return await _context.BlogPosts
			.Where(post => post.IsVisible)
			.OrderByDescending(post => post.CreatedDate)
			.ToListAsync();
	}

	public async Task<BlogPost> AddBlogPost(BlogPostDto dto)
	{
		if (dto.Title is null)
		{
			throw new ArgumentNullException("Title cannot be null.");
		}
		if (dto.Content is null)
		{
			throw new ArgumentNullException("Content cannot be null.");
		}
		if (dto.Title.Trim().Length == 0)
		{
			throw new ArgumentException("Title cannot contain only whitespace or be empty.");
		}
		if (dto.Content.Trim().Length == 0)
		{
			throw new ArgumentException("Content cannot contain only whitespace or be empty.");
		}

		BlogPost blogPost = new()
		{
			Title = dto.Title,
			Content = dto.Content,
			CreatedDate = DateTime.UtcNow,
			IsVisible = true,
		};
		await _context.BlogPosts.AddAsync(blogPost);
		await _context.SaveChangesAsync();

		return blogPost;
	}

	public async Task<BlogPost?> GetBlogPost(int id)
	{
		return await _context.BlogPosts
			.FirstOrDefaultAsync(post => post.BlogPostId == id && post.IsVisible);
	}

	public async Task<bool> DeleteBlogPost(int id)
	{
		var post = await _context.BlogPosts.FirstOrDefaultAsync(post => post.BlogPostId == id);

		if (post is not null)
		{
			post.IsVisible = false;
			await _context.SaveChangesAsync();
			return true;
		}
		return false;
	}
}
