using API;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Product.IntegrationTest
{
	public class IntegrationTest
	{
		protected readonly HttpClient TestClient;

		public IntegrationTest()
		{
			var appFactory = new WebApplicationFactory<Startup>();
			TestClient = appFactory.CreateClient();
		}
	}
}
