using System.ComponentModel.DataAnnotations;

namespace Blog.Api.Models;

public class BlogPost
{
	public int BlogPostId { get; set; }
	[Required]
	public required string Title { get; set; }
	[Required]
	public required string Content { get; set; }
	public DateTime CreatedDate { get; set; }
	public required bool IsVisible { get; set; }
}
