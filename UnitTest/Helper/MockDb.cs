using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Helper
{
	public class MockDb
	{
		private readonly ILogger<MockDb> _logger;

		public MockDb(ILogger<MockDb> logger)
		{
			_logger = logger;
		}

		public DataContext GetTestDbContext()
		{
			var options = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase("InMemoryDbForUnitTesting")
				.Options;

			var db = new DataContext(options);
			db.Database.EnsureCreated();

			try
			{
				if(db.Products.Count() == 0 && db.ProductOptions.Count() == 0)
				{
					SeedTestData.InitializeDbForTests(db);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured seeding the database, Error: {Message}", ex.Message);
			}

			return db;
		}
	}
}
