using API.Controllers;
using Application.Helper;
using Application.ProductOptions;
using Application.Products;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Linq;
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.API
{
	public class ProductControllerPutTest
	{
		private readonly ProductsController sut;
		private readonly Mock<IMediator> mediator = new Mock<IMediator>();

		public ProductControllerPutTest()
		{
			sut = new ProductsController(mediator.Object);
		}

		/// <summary>
		/// Controller should return Ok response when it successfully update the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Put_ValidProduct_OkResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var newProduct = new Product
			{
				Id = product.Id,
				Name = product.Name,
				Description = "Updated Description",
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};

			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Update(Guid.Parse(id), product).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return bad request response when it fails to update the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Post_FailedToUpdate_BadRequestResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var newProduct = new Product
			{
				Id = product.Id,
				Name = product.Name,
				Description = "Updated Description",
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};

			var result = Result<Unit>.Failure("Failed to create product");

			mediator.Setup(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Update(Guid.Parse(id), product).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}

		/// <summary>
		/// Controller should return not found response when it fails to find the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Post_ProductIdIsWrong_NotFoundResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var newProduct = new Product
			{
				Id = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33"),
				Name = product.Name,
				Description = product.Description,
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};

			Result<Unit> result = null;

			mediator.Setup(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Update(Guid.Parse(id), product).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Edit.Command>(y => y.Product == product && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}


		/// <summary>
		/// Controller should return Ok response when it successfully update the product option.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void UpdateOption_UpdateSuccess_OkResponse()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = "Updated Description",
				ProductId = productOption.ProductId
			};

			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.UpdateOption(newProductOption.Id, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption.Equals(newProductOption)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return Not Found response when Mediator returns null.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void UpdateOption_MediatorReturnNull_NotFoundResponse()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = "Updated Description",
				ProductId = productOption.ProductId
			};

			Result<Unit> result = null;

			mediator.Setup(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.UpdateOption(newProductOption.Id, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption == newProductOption), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}

		/// <summary>
		/// Controller should return Bad Request response when Mediator returns an error.
		/// Endpoint: PUT /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void UpdateOption_MediatorReturnError_BadRequestResponse()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = "Updated Description",
				ProductId = productOption.ProductId
			};

			var result = Result<Unit>.Failure("Failed to edit product option");

			mediator.Setup(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.UpdateOption(newProductOption.Id, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionEdit.Command>(y => y.Id == newProductOption.Id && y.ProductOption == newProductOption), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}

	}
}
