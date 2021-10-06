using FluentAssertions;
using IntegrationTest.Helper;
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

namespace IntegrationTest
{
	/// <summary>
	/// Product Controller Integration Test Class.
	/// </summary>
	public class ProductsControllerTest : IntegrationTest
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
			var expectedProducts = SeedTestData.Products;

			//Act
			var response = await TestClient.GetAsync(ApiRoutes.Products.Get);
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
		public async Task Get_ValidProductName_ProductFoundSuccessful()
		{
			//Arrange
			var expectedProducts = SeedTestData.Products.Where(p => p.Name.Contains("Apple")).ToList();

			//Act
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetProductByName.Replace("{name}", "Apple"));
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetProductByName.Replace("{name}", productName));
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
			var expectedProduct = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			//Act
			var response = await TestClient.GetAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
			var result = await response.Content.ReadAsStringAsync();

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

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
			var response = await TestClient.PostAsync(ApiRoutes.Products.Post, content);
			var getProductResponse = await TestClient.GetAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", product.Id.ToString()));
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
			var response = await TestClient.PostAsync(ApiRoutes.Products.Post, content);

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
			var response = await TestClient.PostAsync(ApiRoutes.Products.Post, content);

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
			var response = await TestClient.PostAsync(ApiRoutes.Products.Post, content);

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
			var response = await TestClient.PostAsync(ApiRoutes.Products.Post, content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should update correct product.
		/// Endpoint: PUT /products/{id}
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")]
		[InlineData("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3")]
		public async Task Put_ValidProduct_ProductUpdateSuccessful(string id)
		{
			//Arrange
			var expectedProduct = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
			
			//Randomly change the product attribute.
			Random rand = new Random();
			if (rand.Next(0, 2) == 0)
			{
				expectedProduct.Description = "Updated Description"; 
			}
			else
			{
				expectedProduct.Price += 10M; 
			}

			JsonContent content = JsonContent.Create(expectedProduct);

			//Act
			var response = await TestClient.PutAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id), content);
			var getProductResponse = await TestClient.GetAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));
			var product = JsonConvert.DeserializeObject<Product>(await getProductResponse.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			product.Should().Be(expectedProduct);
		}

		/// <summary>
		/// Controller should not update the product when the product ID is invalid.
		/// Endpoint: PUT /products/{id}
		/// </summary>
		/// <param name="id">Product ID</param>
		/// <returns></returns>
		[Theory]
		[InlineData("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")]
		public async Task Put_InvalidProduct_BadRequestResponse(string id)
		{
			//Arrange
			var expectedProduct = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();
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
			var response = await TestClient.PutAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", invalidId.ToString()), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

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
			var response = await TestClient.DeleteAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
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
			var response = await TestClient.DeleteAsync(ApiRoutes.Products.ProducIdUrl.Replace("{id}", id));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetOption.Replace("{id}", id));
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetOption.Replace("{id}", id));
			var productsOption = JsonConvert.DeserializeObject<ProductOptions>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Items.Count.Should().Be(0);
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
			var productOptionExpected = SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse(optionId)).FirstOrDefault();

			//Act
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));
			var productsOption = JsonConvert.DeserializeObject<ProductOption>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Should().BeEquivalentTo(productOptionExpected);
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
			var response = await TestClient.GetAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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

			JsonContent content = JsonContent.Create(newProductOption);

			//Act
			var response = await TestClient.PostAsync(ApiRoutes.Products.GetOption.Replace("{id}", newProductOption.ProductId.ToString()), content);
			var productOptionResponse = await TestClient.GetAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", newProductOption.ProductId.ToString())
				.Replace("{optionId}", newProductOption.Id.ToString()));

			var productsOption = JsonConvert.DeserializeObject<ProductOption>(await productOptionResponse.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			productsOption.Should().BeEquivalentTo(newProductOption);
		}

		/// <summary>
		/// Controller should not create the option when the given product ID is invalid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task CreateOption_InvalidProductID_BadRequestResponse()
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
			var response = await TestClient.PostAsync(ApiRoutes.Products.GetOption.Replace("{id}", newProductOption.ProductId.ToString()), content);

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
			var response = await TestClient.PostAsync(ApiRoutes.Products.GetOption.Replace("{id}", "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

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
			var productOption = SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse(optionId)).FirstOrDefault();

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
			var response = await TestClient.PutAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);
			var updatedProductOption = new ProductOption();

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			updatedProductOption.Should().BeEquivalentTo(productOption);
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
			var response = await TestClient.PutAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
			var response = await TestClient.PutAsync(ApiRoutes.Products.GetOptionId.Replace("{id}", productId).Replace("{optionId}", optionId), content);

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should delete the correct option.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_ValidProductIDAndOptionID_OptionDeletionSuccessful()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();

			//Act
			var response = await TestClient.DeleteAsync(ApiRoutes.Products.GetOption.Replace("{id}", productOption.ProductId.ToString()).Replace("{optionId}", productOption.Id.ToString()));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should not delete the option when the option id is invalid.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_ValidProductIDAndInvalidOption_BadRequestResponse()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();
			var invalidOptionId = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F33";

			//Act
			var response = await TestClient.DeleteAsync(ApiRoutes.Products.GetOption.Replace("{id}", productOption.ProductId.ToString()).Replace("{optionId}", invalidOptionId));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		/// <summary>
		/// Controller should not delete the option when the product id is invalid.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task DeleteOption_InvalidProductIDAndValidOptionID_BadRequestResponse()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(p => p.Id == Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03")).FirstOrDefault();
			var invalidProductId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";

			//Act
			var response = await TestClient.DeleteAsync(ApiRoutes.Products.GetOption.Replace("{id}", invalidProductId).Replace("{optionId}", productOption.Id.ToString()));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
	}
}
