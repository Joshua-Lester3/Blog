﻿using Blog.Api.Data;
using Blog.Api.Dtos;
using Blog.Api.Models;
using Blog.Api.Services;

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
	public async Task GetBlogList_AddedOnePost_ReturnsListLengthOne()
	{
		// Arrange
		await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		// Act
		var blogList = await _service.GetBlogList();

		// Assert
		Assert.IsTrue(blogList.Count() == 1);
	}

	[TestMethod]
	public async Task GetBlogList_AddedThreePosts_ReturnsReverseOrderedListByCreatedDate()
	{
		// Arrange
		var post1 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		var post2 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 2",
			Content = "When asked what he believed in, a viking once said...",
		});

		var post3 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 3",
			Content = "Zshk zshk zshk... Is his Majesty King Sweyn awake?",
		});

		var posts = new BlogPost[] { post3, post2, post1 };

		// Act
		var blogList = await _service.GetBlogList();

		// Assert
		CollectionAssert.AreEqual(posts, blogList);
	}

	[TestMethod]
	public async Task GetBlogList_ThreePostsWithOneRemoved_ReturnsTwoPosts()
	{
		// Arrange
		var post1 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		post1.IsVisible = false;
		await _context.SaveChangesAsync();

		var post2 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 2",
			Content = "When asked what he believed in, a viking once said...",
		});

		var post3 = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 3",
			Content = "Zshk zshk zshk... Is his Majesty King Sweyn awake?",
		});

		var posts = new BlogPost[] { post3, post2 };

		// Act
		var blogList = await _service.GetBlogList();

		// Assert
		CollectionAssert.AreEqual(posts, blogList);
	}

	[TestMethod]
	public async Task PostBlogPost_NonemptyNonnullDtoFields_SuccessfullyAdds()
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north... Beyond the frozen sea... ...They come, bringing with them the black clouds of war."
		};

		// Act
		BlogPost post = await _service.PostBlogPost(dto);

		// Assert
		CollectionAssert.Contains(_context.BlogPosts.ToList(), post);
	}

	[TestMethod]
	[DataRow(null, "From the distant north...")]
	[DataRow("Vinland Saga Vol. 1", null)]
	public async Task PostBlogPost_NullDtoFields_ThrowsArgumentException(string title, string content)
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = title,
			Content = content
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _service.PostBlogPost(dto));
	}

	[TestMethod]
	[DataRow("", "From the distant north...")]
	[DataRow("Vinland Saga Vol. 1", "")]
	public async Task PostBlogPost_EmptyDtoFields_ThrowsArgumentException(string title, string content)
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = title,
			Content = content
		};

		// Act

		// Assert
		await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _service.PostBlogPost(dto));
	}

	[TestMethod]
	public async Task GetBlogPost_ValidId_Success()
	{
		// Arrange
		var post = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		// Act
		var result = await _service.GetBlogPost(post.BlogPostId);

		// Assert
		Assert.AreEqual(post, result);
	}

	[TestMethod]
	public async Task GetBlogPost_NotVisible_ReturnsNull()
	{
		// Arrange
		var post = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});
		post.IsVisible = false;
		await _context.SaveChangesAsync();

		// Act
		var result = await _service.GetBlogPost(post.BlogPostId);

		// Assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public async Task GetBlogPost_InvalidId_ReturnsNull()
	{
		// Arrange
		var post = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		// Act
		var result = await _service.GetBlogPost(-1);

		// Assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public async Task DeleteBlogPost_PostExists_Success()
	{
		// Arrange
		var post = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});

		// Act
		await _service.DeleteBlogPost(post.BlogPostId);

		// Assert
		Assert.IsFalse(post.IsVisible);
	}

	[TestMethod]
	public async Task DeleteBlogPost_PostDoesNotExist_Success()
	{
		// Arrange

		// Act

		// Assert
		Assert.IsFalse(await _service.DeleteBlogPost(-1));
	}

	[TestMethod]
	public async Task PostBlogPost_PostExists_Success()
	{
		// Arrange
		var post = await _service.PostBlogPost(new BlogPostDto
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		});
		var dto = new BlogPostDto
		{
			BlogPostId = post.BlogPostId,
			Title = "Vinland Saga Vol. 2",
			Content = "When asked what he believed in, a viking once said...",
		};

		// Act
		post = await _service.PostBlogPost(dto);

		// Assert
		Assert.AreEqual(dto.Title, post.Title);
		Assert.AreEqual(dto.Content, post.Content);
	}
}
