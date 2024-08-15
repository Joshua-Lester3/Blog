using Blog.Api.Dtos;
using Blog.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata;

namespace Blog.Api.Tests;

[TestClass]
public class BlogControllerTests
{
	private static readonly WebApplicationFactory<Program> _factory = new();
	private HttpClient _httpClient = null!; // Will be set in TestInitialize

	[TestInitialize]
	public void Init()
	{
		_httpClient = _factory.CreateClient();
	}

	[TestMethod]
	public async Task GetBlogList_NormalConditions_StatusCodeIsOK()
	{
		// Arrange


		// Act
		var response = await _httpClient.GetAsync("/blog/getBlogList");

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task GetBlogList_NormalConditions_ReturnsList()
	{
		// Arrange

		// Act
		var response = await _httpClient.GetAsync("/blog/getBlogList");
		var content = await response.Content.ReadFromJsonAsync<List<BlogPost>>();

		// Assert
		Assert.IsNotNull(content);
	}

	[TestMethod]
	public async Task AddBlogPost_NormalConditions_ReturnsAddedBlogPost()
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		};
		var content = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/blog/postBlogPost", content);
		var blogPost = await response.Content.ReadFromJsonAsync<BlogPost>();

		// Assert
		Assert.IsNotNull(blogPost);
		Assert.AreEqual(dto.Title, blogPost.Title);
		Assert.AreEqual(dto.Content, blogPost.Content);
	}

	[TestMethod]
	[DataRow(null, "From the distant north...")]
	[DataRow("Vinland Saga Vol. 1", null)]
	public async Task AddBlogPost_NullTitleOrContent_ReturnsStatusCode400(string title, string content)
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = title,
			Content = content,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/blog/postBlogPost", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	[DataRow("", "From the distant north...")]
	[DataRow("Vinland Saga Vol. 1", "")]
	public async Task AddBlogPost_EmptyTitleOrContent_ReturnsStatusCode400(string title, string content)
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = title,
			Content = content,
		};
		var jsonContent = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/blog/postBlogPost", jsonContent);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[TestMethod]
	public async Task GetBlogPost_ValidBlogPostId_Success()
	{
		// Arrange
		var added = await AddBlogPost();
		var url = $"/blog/getBlogPost?id={added!.BlogPostId}";

		// Act
		var response = await _httpClient.GetAsync(url);
		var blogPost = await response.Content.ReadFromJsonAsync<BlogPost>();

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
		Assert.IsNotNull(blogPost);
	}

	[TestMethod]
	public async Task GetBlogPost_InvalidBlogPostId_StatusCodeIsBadRequest()
	{
		// Arrange
		var url = $"/blog/getBlogPost?id=-1";

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}

	private async Task<BlogPost?> AddBlogPost()
	{
		// Arrange
		BlogPostDto dto = new()
		{
			Title = "Vinland Saga Vol. 1",
			Content = "From the distant north...",
		};
		var content = JsonContent.Create(dto);

		// Act
		var response = await _httpClient.PostAsync("/blog/postBlogPost", content);
		var blogPost = await response.Content.ReadFromJsonAsync<BlogPost>();

		// Assert
		Assert.IsNotNull(blogPost);
		return blogPost;
	}

	[TestMethod]
	public async Task DeleteBlogPost_PostExists_Success()
	{
		// Arrange
		var added = (await AddBlogPost())!;

		// Act
		var response = await _httpClient.PostAsync($"/blog/deleteBlogPost/{added.BlogPostId}", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
	}

	[TestMethod]
	public async Task DeleteBlogPost_PostDoesNotExist_BadRequest()
	{
		// Arrange

		// Act
		var response = await _httpClient.PostAsync("/blog/deleteBlogPost/-1", null);

		// Assert
		Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
	}
}
