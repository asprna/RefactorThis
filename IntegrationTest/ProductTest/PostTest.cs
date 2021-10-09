using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using System.Net.Http.Json;
using Domain;

namespace IntegrationTest.ProductTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class PostTest : helper.IntegrationTest
	{
		/// <summary>
		/// Controller should create a new product when the given product is valid.
		/// Endpoint: POST /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_ValidProduct_ProductCreationSuccessful()
		{
			//Arrange
			Product product = new Product { Id = Guid.NewGuid(), Name = "Apple iPhone 8", Price = 1299.99M, DeliveryPrice = 15.99M, Description = "Newest mobile product from Apple." };
			JsonContent content = JsonContent.Create(product);
			
			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.Post, content);
			var getProductResponse = await TestClient.GetAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", product.Id.ToString()));
			var newProduct = JsonConvert.DeserializeObject<Product>(await getProductResponse.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			newProduct.Should().BeEquivalentTo(product);
		}

		/// <summary>
		/// Controller should not create a new product when the given product is invalid.
		/// Endpoint: POST /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_EmptyProducts_BadRequestResponse()
		{
			//Arrange
			Product product = new Product();
			JsonContent content = JsonContent.Create(product);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.Post, content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should validate the product against validation rule - Name.
		/// Endpoint: POST /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_InputValidationProductsNoName_BadRequestResponse()
		{
			//Arrange
			Product product = new Product
			{
				Id = Guid.NewGuid(),
				Name = "",
				Description = "Description",
				DeliveryPrice = 10.00M,
				Price = 100.00M
			};
			JsonContent content = JsonContent.Create(product);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.Post, content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should validate the product against validation rule - Id.
		/// Endpoint: POST /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_InputValidationProductsNoId_BadRequestResponse()
		{
			//Arrange
			Product product = new Product
			{
				Id = Guid.Empty,
				Name = "Name",
				Description = "Description",
				DeliveryPrice = 10.00M,
				Price = 100.00M
			};
			JsonContent content = JsonContent.Create(product);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.Post, content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should validate the product against validation rule - Description.
		/// Endpoint: POST /products
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Post_InputValidationProductsNoDescription_BadRequestResponse()
		{
			//Arrange
			Product product = new Product
			{
				Id = Guid.NewGuid(),
				Name = "Name",
				Description = "",
				DeliveryPrice = 10.00M,
				Price = 100.00M
			};
			JsonContent content = JsonContent.Create(product);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.Post, content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
	}
}
