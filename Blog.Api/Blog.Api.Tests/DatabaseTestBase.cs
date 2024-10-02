using Microsoft.EntityFrameworkCore;
using Blog.Api.Data;
using Microsoft.Data.Sqlite;

namespace Blog.Api.Tests;
public abstract class DatabaseTestBase
{
	private SqliteConnection SqliteConnection { get; set; } = null!;
	protected DbContextOptions<AppDbContext> Options { get; private set; } = null!;

	[TestInitialize]
	public void InitializeDb()
	{
		SqliteConnection = new SqliteConnection("DataSource=:memory:");
		SqliteConnection.Open();

		Options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(SqliteConnection)
			.Options;

		using var context = new AppDbContext(Options);
		context.Database.EnsureCreated();
	}

	[TestCleanup]
	public void CloseDbConnection()
	{
		SqliteConnection.Close();
	}
}