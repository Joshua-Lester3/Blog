using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api.Services;

public class BlogService
{
	private readonly AppDbContext _context;
	private static object _postingBlogPostLock = new();

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

	public async Task<BlogPost> PostBlogPost(BlogPostDto dto)
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

		BlogPost? foundBlogPost = null;
		if (dto.BlogPostId != -1)
		{
			foundBlogPost = await _context.BlogPosts
				.FirstOrDefaultAsync(post => post.BlogPostId == dto.BlogPostId);
		}
		if (foundBlogPost is null)
		{
			lock (_postingBlogPostLock)
			{
				if (dto.BlogPostId != -1)
				{
					foundBlogPost = _context.BlogPosts
						.FirstOrDefault(post => post.BlogPostId == dto.BlogPostId);
				}

				if (foundBlogPost is null)
				{
					BlogPost blogPost = new()
					{
						Title = dto.Title,
						Content = dto.Content,
						CreatedDate = DateTime.UtcNow,
						IsVisible = true,
					};
					_context.BlogPosts.Add(blogPost);
					_context.SaveChanges();
					return blogPost;
				} else
				{
					foundBlogPost.Title = dto.Title;
					foundBlogPost.Content = dto.Content;
					_context.SaveChanges();
					return foundBlogPost;
				}
			}
		} else
		{
			lock (_postingBlogPostLock)
			{
				foundBlogPost.Title = dto.Title;
				foundBlogPost.Content = dto.Content;
				_context.SaveChanges();
				return foundBlogPost;
			}
		}
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
