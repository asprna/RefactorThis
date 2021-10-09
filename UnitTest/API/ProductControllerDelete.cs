using API.Controllers;
using Application.Helper;
using Application.ProductOptions;
using Application.Products;
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
	public class ProductControllerDelete
	{
		private readonly ProductsController sut;
		private readonly Mock<IMediator> mediator = new Mock<IMediator>();

		public ProductControllerDelete()
		{
			sut = new ProductsController(mediator.Object);
		}

		/// <summary>
		/// Controller should return Ok response when it successfully delete the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Delete_ValidProduct_OkResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			
			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Delete(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return bad request response when it fails to delete the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Delete_FailedToDelete_BadRequestResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			
			var result = Result<Unit>.Failure("Failed to create product");

			mediator.Setup(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Delete(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}

		/// <summary>
		/// Controller should return not found response when it fails to find the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Delete_ProductIdIsWrong_NotFoundResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33";

			Result<Unit> result = null;

			mediator.Setup(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Delete(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Delete.Command>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}

		/// <summary>
		/// Controller should return Ok response when it successfully delete the product option.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void DeleteOption_ValidProductOption_OkResponse()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03";

			var result = Result<Unit>.Success(Unit.Value);

			mediator.Setup(x => x.Send(It.Is<ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.DeleteOption(Guid.Parse(productId), Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is< ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return bad request response when it fails to delete the product.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void DeleteOption_FailedToDelete_BadRequestResponse()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03";

			var result = Result<Unit>.Failure("Failed to create product");

			mediator.Setup(x => x.Send(It.Is<ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.DeleteOption(Guid.Parse(productId), Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<BadRequestObjectResult>();
		}

		/// <summary>
		/// Controller should return not found response when it fails to find the product.
		/// Endpoint: DELETE /products/{id}/options/{optionId}
		/// </summary>
		[Fact]
		public void DeleteOption_ProductIdIsWrong_NotFoundResponse()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03";

			Result<Unit> result = null;

			mediator.Setup(x => x.Send(It.Is<ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.DeleteOption(Guid.Parse(productId), Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionDelete.Command>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}
	}
}
