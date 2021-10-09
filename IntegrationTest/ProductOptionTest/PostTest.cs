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
using AutoMapper;
using Application.Helper;

namespace IntegrationTest.ProductOptionTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class PostTest : helper.IntegrationTest
	{
		private readonly IMapper _mapper;

		public PostTest()
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			_mapper = mappingConfig.CreateMapper();
		}

		/// <summary>
		/// Controller should add the product option when the option is valid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task CreateOption_ValidProductOption_OptionCreationSuccessful()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var newProductOptionDTO = _mapper.Map<ProductOption, ProductOptionDTO>(newProductOption);

			JsonContent content = JsonContent.Create(newProductOption);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", newProductOption.ProductId.ToString()), content);
			var productOptionResponse = await TestClient.GetAsync(helper.ApiRoutes.Products.GetOptionId.Replace("{id}", newProductOption.ProductId.ToString())
				.Replace("{optionId}", newProductOption.Id.ToString()));

			var productsOption = JsonConvert.DeserializeObject<ProductOption>(await productOptionResponse.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Should().BeEquivalentTo(newProductOptionDTO);
		}

		/// <summary>
		/// Controller should not create the option when the given product ID is invalid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task CreateOption_InvalidProductID_NotFoundResponse()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			JsonContent content = JsonContent.Create(newProductOption);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", newProductOption.ProductId.ToString()), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		/// <summary>
		/// Controller should not create the option when the given product ID is invalid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task CreateOption_ProductIDMissMatch_BadRequestResponse()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			JsonContent content = JsonContent.Create(newProductOption);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should not create the option when the given option is invalid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task CreateOption_NoOptionProvided_BadRequestResponse()
		{
			//Arrange
			var newProductOption = new ProductOption();
			JsonContent content = JsonContent.Create(newProductOption);

			//Act
			var response = await TestClient.PostAsync(helper.ApiRoutes.Products.GetOption.Replace("{id}", "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
	}
}
