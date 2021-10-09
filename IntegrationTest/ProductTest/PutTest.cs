using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Domain;
using Xunit;

namespace IntegrationTest.ProductTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class PutTest : helper.IntegrationTest
	{
		/// <summary>
		/// Controller should update correct product.
		/// Endpoint: PUT /products/{id}
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns></returns>
		[Fact]
		public async Task Put_ValidProduct_ProductUpdateSuccessful()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = helper.SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var expectedProduct = new Product
			{
				Id = product.Id,
				Name = product.Name,
				Description = "Updated Description",
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};

			JsonContent content = JsonContent.Create(expectedProduct);

			//Act
			var response = await TestClient.PutAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		/// <summary>
		/// Controller should not update the product when the product ID is invalid.
		/// Endpoint: PUT /products/{id}
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")]
		public async Task Put_InvalidProduct_NotFoundResponse(string id)
		{
			//Arrange
			var expectedProduct = helper.SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
			var invalidId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133");
			var productWithInvalidId = new Product
			{
				Id = invalidId,
				Name = expectedProduct.Name,
				Price = expectedProduct.Price,
				Description = expectedProduct.Description,
				DeliveryPrice = expectedProduct.DeliveryPrice
			};
			JsonContent content = JsonContent.Create(expectedProduct);

			//Act
			var response = await TestClient.PutAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", invalidId.ToString()), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
