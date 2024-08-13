using Blog.Api.Data;

namespace Blog.Api.Services;

public class BlogService
{
	private readonly AppDbContext _context;

	public BlogService(AppDbContext context)
	{
		_context = context;
	}

	public List<string> GetBlogList()
	{
		return [];
	}
}
