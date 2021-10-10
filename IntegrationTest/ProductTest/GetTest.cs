using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Domain;

namespace IntegrationTest.ProductTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class GetTest : helper.IntegrationTest
	{

		/// <summary>
		/// Controller should return all the product.
		/// Endpoint: GET /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_ReturnAllProducts()
		{
			//Arrange
			var expectedProducts = helper.SeedTestData.Products;

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.Get);
			var products = JsonConvert.DeserializeObject<Products>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			products.Items.Count.Should().Be(expectedProducts.Count);
			products.Items.Should().Contain(expectedProducts);
		}

		/// <summary>
		/// Controller should find all products matching the specified name.
		/// Endpoint: GET /products?name={name}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_ValidProductName_ProductFoundSuccess()
		{
			//Arrange
			var expectedProducts = helper.SeedTestData.Products.Where(p => p.Name.Contains("Apple")).ToList();

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetProductByName.Replace("{name}", "Apple"));
			var products = JsonConvert.DeserializeObject<Products>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			products.Items.Count.Should().Be(expectedProducts.Count);
			products.Items.Should().Contain(expectedProducts);
		}

		/// <summary>
		/// Controller should not return a product when the product name is invalid.
		/// Endpoint: GET /products?name={name}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_InvalidProductName_NoProductFound()
		{
			//Arrange
			var productName = "No product";

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetProductByName.Replace("{name}", productName));
			var products = JsonConvert.DeserializeObject<Products>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			products.Items.Count.Should().Be(0);
		}

		/// <summary>
		/// Controller should find the correct product that matches the given product ID.
		/// Endpoint: GET /products/{id}
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3")]
		public async Task Get_ValidProducId_ProductFoundSuccessful(string id)
		{
			//Arrange
			var expectedProduct = helper.SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
			var product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			product.Should().Be(expectedProduct);
		}

		/// <summary>
		/// Controller should not find a product when the product ID is invalid.
		/// Endpoint: GET /products/{id}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Get_InvalidProductId_NoProductFound()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
