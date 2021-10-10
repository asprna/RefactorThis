using FluentAssertions;
using helper = IntegrationTest.Helper;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest.ProductTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class DeleteTest : helper.IntegrationTest
	{
		/// <summary>
		/// Controller should delete the correct product.
		/// Endpoint: DELETE /products/{id}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Delete_ValidProductId_ProductDeletionSuccessful()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";

			//Act
			var response = await TestClient.DeleteAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
			var getProductResponse = await TestClient.GetAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
			
			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			getProductResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		/// <summary>
		/// Controller should not delete the product when the given product id in invalid.
		/// Endpoint: DELETE /products/{id}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task Delete_InvalidId_BadRequestResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";

			//Act
			var response = await TestClient.DeleteAsync(helper.ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
