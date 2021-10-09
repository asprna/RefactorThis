using FluentAssertions;
using helper = IntegrationTest.Helper;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Domain;
using AutoMapper;
using Application.Helper;
using Newtonsoft.Json;

namespace IntegrationTest.ProductOptionTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class GetTest : helper.IntegrationTest
	{
		private readonly IMapper _mapper;

		public GetTest()
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			_mapper = mappingConfig.CreateMapper();
		}

		/// <summary>
		/// Controller should return all the options for the given product ID
		/// Endpoint: GET /products/{id}/options
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <param name="count">Expected number of options count.</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3", 2)]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3", 3)]
		public async Task GetOptions_AllProductsOptions_AllOptionsReturnedSuccessful(string id, int count)
		{
			//Arrange

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", id));
			var productsOption = JsonConvert.DeserializeObject<ProductOptions>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Items.Count.Should().Be(count);
		}

		/// <summary>
		/// Controller should not return any options when the product ID is invalid.
		/// Endpoint: GET /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task GetOptions_InvalidProductId_NoOptionFound()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", id));
			var productsOption = JsonConvert.DeserializeObject<ProductOptions>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		/// <summary>
		/// Controller should find the specified product option for the specified product.
		/// Endpoint: GET /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId">Product ID</param>
		/// <param name="optionId">Option ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3", "0643CCF0-AB00-4862-B3C5-40E2731ABCC9")]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3", "5C2996AB-54AD-4999-92D2-89245682D534")]
		public async Task GetOption_ValidProductIDAndOptionID_ProductOptionFoundSuccessful(string productId, string optionId)
		{
			//Arrage
			var productOptionExpected = helper.SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse(optionId)).FirstOrDefault();
			var productOptionExpectedDTO = _mapper.Map<ProductOption, ProductOptionDTO>(productOptionExpected);

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));
			var productsOption = JsonConvert.DeserializeObject<ProductOption>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Should().BeEquivalentTo(productOptionExpectedDTO);
		}

		/// <summary>
		/// Controller should not return a product option when the given option is invalid.
		/// Endpoint: GET /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="optionId"></param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3", "0643CCF0-AB00-4862-B3C5-40E2731ABC99")]
		public async Task GetOption_ValidProductIDAndInvalidOptionId_BadRequestResponse(string productId, string optionId)
		{
			//Arrage

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		/// <summary>
		/// Controller should not return a product option when the given product id is invalid.
		/// Endpoint: GET /products/{id}/options/{optionId}
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="optionId"></param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB133", "0643CCF0-AB00-4862-B3C5-40E2731ABCC9")]
		public async Task GetOption_InvalidProductIDAndValidOptionID_BadRequestResponse(string productId, string optionId)
		{
			//Arrage

			//Act
			var response = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
