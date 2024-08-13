using Blog.Api.Data;
using Blog.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Tests;

[TestClass]
public class BlogServiceTests : DatabaseTestBase
{
	private BlogService _service = null!;
	private AppDbContext _context = null!;

	[TestInitialize]
	public void Init()
	{
		_context = new AppDbContext(Options);
		_service = new BlogService(_context);
	}

	[TestMethod]
	public void GetBlogList_NormalConditions_ReturnsList()
	{
		// Arrange

		// Act

		// Assert
		Assert.IsNotNull(_service.GetBlogList());
	}
}
