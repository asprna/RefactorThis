﻿using API;
using IntegrationTest.Helper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTest
{
	public class IntegrationTest
	{
		protected readonly HttpClient TestClient;

		/// <summary>
		/// Create Web Application Factory for Integration Unit test.
		/// </summary>
		public IntegrationTest()
		{
			var appFactory = new WebApplicationFactory<Startup>()
				.WithWebHostBuilder(builder =>
					{
						builder.ConfigureServices(services =>
						{
							var descriptior = services.SingleOrDefault(
								d => d.ServiceType == typeof(DataContext)
							);

							services.Remove(descriptior);
							services.AddDbContext<DataContext>(opt =>
							{
								opt.UseInMemoryDatabase("InMemoryDbForTesting");
							});

							var sp = services.BuildServiceProvider();

							using(var scope = sp.CreateScope())
							{
								var scopedService = scope.ServiceProvider;
								var db = scopedService.GetRequiredService<DataContext>();
								var logger = scopedService.GetRequiredService<ILogger<IntegrationTest>>();

								db.Database.EnsureCreated();

								try
								{
									SeedTestData.InitializeDbForTests(db);
								}
								catch(Exception ex)
								{
									logger.LogError(ex, "An Error occured seeding the database, Error: {Message}", ex.Message);
								}
							}
						});
					}
				
				);
			TestClient = appFactory.CreateClient();
		}
	}
}
