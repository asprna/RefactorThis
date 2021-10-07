using API.Controllers;
using Application.Helper;
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
	}
}
