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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTest.Helper;
using Xunit;


namespace UnitTest.API
{
	public class ProductControllerPostTest
	{
		private readonly ProductsController sut;
		private readonly Mock<IMediator> mediator = new Mock<IMediator>();

		public ProductControllerPostTest()
		{
			sut = new ProductsController(mediator.Object);
		}

		/// <summary>
		/// Controller should return Ok response when it successfully adds the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Post_ValidProduct_OkResponse()
		{
			//Arrange
			var product = new Product
			{
				Id = Guid.NewGuid(),
				Name = "Post Test Product",
				Description = "Test product only",
				Price = 1000.00M,
				DeliveryPrice = 10.00M
			};

			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<Create.Command>(y => y.Product == product), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Post(product).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Create.Command>(y => y.Product == product), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
		}

		/// <summary>
		/// Controller should return bad request response when it fails to add the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Post_FailedToAdd_BadRequestResponse()
		{
			//Arrange
			var product = new Product
			{
				Id = Guid.NewGuid(),
				Name = "Post Test Product",
				Description = "Test product only",
				Price = 1000.00M,
				DeliveryPrice = 10.00M
			};

			var result = Result<Unit>.Failure("Failed to create product");

			mediator.Setup(x => x.Send(It.Is<Create.Command>(y => y.Product == product), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Post(product).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Create.Command>(y => y.Product == product), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}

		/// <summary>
		/// Controller should return Ok response when it successfully adds the product option.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		[Fact]
		public void CreateOption_ValidProductOption_OkResponse()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.CreateOption(newProductOption.ProductId, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return Not Found response when the product id is invalid.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		[Fact]
		public void CreateOption_InValidProductId_NoFoundResponse()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			Result<Unit> result = null;

			mediator.Setup(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.CreateOption(newProductOption.ProductId, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}

		/// <summary>
		/// Controller should return Bad response when Mediator failed.
		/// Endpoint: POST /products/{id}/options
		/// </summary>
		[Fact]
		public void CreateOption_MediatorFailed_BadResponse()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var result = Result<Unit>.Failure("Failed to create product option");

			mediator.Setup(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.CreateOption(newProductOption.ProductId, newProductOption).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionCreate.Command>(y => y.ProductId == newProductOption.ProductId && y.ProductOption == newProductOption), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}
	}
}
