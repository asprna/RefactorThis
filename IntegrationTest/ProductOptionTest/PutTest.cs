using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
//using model = API.Models;
using Xunit;
//using API.Models;
using System.Net.Http.Json;
using Domain;

namespace IntegrationTest.ProductOptionTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class PutTest : helper.IntegrationTest
	{
		/// <summary>
		/// Controller should update the correct product option.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId">Product ID</param>
		/// <param name="optionId">Option ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3", "0643CCF0-AB00-4862-B3C5-40E2731ABCC9")]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3", "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")]
		public async Task UpdateOption_ValidProductIDAndValidOptionID_OptionUpdatedSuccessful(string productId, string optionId)
		{
			//Arrange
			var productOption = helper.SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse(optionId)).FirstOrDefault();

			Random rand = new Random();
			if (rand.Next(0, 2) == 0)
			{
				productOption.Name = "New Namen";
			}
			else
			{
				productOption.Description = "New Description";
			}

			JsonContent content = JsonContent.Create(productOption);

			//Act
			var response = await TestClient.PutAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);
			var responseProductsOption = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));
			var updatedProductsOption = JsonConvert.DeserializeObject<ProductOption>(await responseProductsOption.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			updatedProductsOption.Should().BeEquivalentTo(productOption);
		}

		/// <summary>
		/// Controller should not update the product option when the option id is invalid.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId">Product ID</param>
		/// <param name="optionId">Option ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3", "0643CCF0-AB00-4862-B3C5-40E2731ABC99")]
		public async Task UpdateOption_ValidProductIDAndInvalidOptionID_BadRequestResponse(string productId, string optionId)
		{
			//Arrange
			var productOption = new ProductOption 
			{
				Id = Guid.Parse(optionId),
				ProductId = Guid.Parse(productId),
				Description = "New Description",
				Name = "New Name"
			};

			JsonContent content = JsonContent.Create(productOption);

			//Act
			var response = await TestClient.PutAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		/// <summary>
		/// Controller should not update the product option when the product id is invalid.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId">Product ID</param>
		/// <param name="optionId">Option ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33", "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")]
		public async Task UpdateOption_InvalidProductIDAndValidOptionID_BadRequestResponse(string productId, string optionId)
		{
			//Arrange
			var productOption = new ProductOption
			{
				Id = Guid.Parse(optionId),
				ProductId = Guid.Parse(productId),
				Description = "New Description",
				Name = "New Name"
			};

			JsonContent content = JsonContent.Create(productOption);

			//Act
			var response = await TestClient.PutAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
