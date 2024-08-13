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
		var content = await response.Content.ReadFromJsonAsync<List<string>>();

		// Assert
		Assert.IsNotNull(content);
	}
}
