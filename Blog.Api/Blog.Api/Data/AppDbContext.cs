using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Blog.Api.Models;

namespace Blog.Api.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<BlogPost> BlogPosts { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
