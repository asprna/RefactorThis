using FluentAssertions;
using Product.IntegrationTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using model = API.Models;
using Xunit;
using API.Models;

namespace Product.IntegrationTest
{
	public class ProductsControllerTest : IntegrationTest
	{
		[Fact]
		public async Task Get_ReturnAllProducts()
		{
			//Arrange

			//Act
			var resonse = await TestClient.GetAsync(ApiRoutes.Products.Get);

			//Assert
			resonse.StatusCode.Should().Be(HttpStatusCode.OK);
			JsonConvert.DeserializeObject<Products>(await resonse.Content.ReadAsStringAsync()).Should().NotBeNull();
		}
	}
}
