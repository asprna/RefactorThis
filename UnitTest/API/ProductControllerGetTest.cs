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
	public class ProductControllerGetTest
	{
		private readonly ProductsController sut;
		private readonly Mock<IMediator> mediator = new Mock<IMediator>();

		public ProductControllerGetTest()
		{
			sut = new ProductsController(mediator.Object);
		}

		/// <summary>
		/// Controller should return Ok response when it finds the products.
		/// Endpoint: GET /products
		/// </summary>
		[Fact]
		public void Get_NoParameter_OkResponseWithProduct()
		{
			//Arrange
			var products = new Products
			{
				Items = SeedTestData.Products
			};

			var result = Result<Domain.Products>.Success(products);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(null).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
			var productReturned = okResult.Value.Should().BeAssignableTo<Products>().Subject;
			productReturned.Items.Count().Should().Be(products.Items.Count());
		}

		/// <summary>
		/// Controller should return Ok response with no products when it cannot find the products.
		/// Endpoint: GET /products
		/// </summary>
		[Fact]
		public void Get_ProductsListNull_NotFoundResponse()
		{
			//Arrange
			var products = new Products();

			var result = Result<Domain.Products>.Success(products);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(null).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
			var productReturned = okResult.Value.Should().BeAssignableTo<Products>().Subject;
			productReturned.Items.Should().BeNull();
		}

		/// <summary>
		/// Controller should return Bad Request response when the request fails.
		/// Endpoint: GET /products
		/// </summary>
		[Fact]
		public void Get_MediatorFailed_BadRequestResponse()
		{
			//Arrange
			var error = "Unit Test Failed";
			var result = Result<Domain.Products>.Failure(error);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(null).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == null), It.IsAny<CancellationToken>()));
			var errorReturned = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
			errorReturned.Value.Should().Be(error);
		}

		/// <summary>
		/// Controller should return Ok response when it finds the products by name.
		/// Endpoint: GET /products?name={name}
		/// </summary>
		[Fact]
		public void Get_FilterByName_OkResponseWithProduct()
		{
			//Arrange;
			var name = "Apple";
			var products = new Products
			{
				Items = SeedTestData.Products.Where(x => x.Name.Contains(name)).ToList()
			};

			var result = Result<Domain.Products>.Success(products);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(name).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
			var productReturned = okResult.Value.Should().BeAssignableTo<Products>().Subject;
			productReturned.Items.Count().Should().Be(products.Items.Count());
		}

		/// <summary>
		/// Controller should return Ok response with no products when the invalid name provided.
		/// Endpoint: GET /products?name={name}
		/// </summary>
		[Fact]
		public void Get_FilterInvalidByName_OkResponseWithNoProduct()
		{
			//Arrange
			var name = "NoProduct";
			var products = new Products
			{
				Items = SeedTestData.Products.Where(x => x.Name.Contains(name)).ToList()
			};

			var result = Result<Domain.Products>.Success(products);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(name).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
			var productReturned = okResult.Value.Should().BeAssignableTo<Products>().Subject;
			productReturned.Items.Count().Should().Be(products.Items.Count());
		}

		/// <summary>
		/// Controller should return Bad Request response when the request fails.
		/// Endpoint: GET /products?name={name}
		/// </summary>
		[Fact]
		public void Get_MediatorFailedWhileFilteringProductName_BadRequestResponse()
		{
			//Arrange
			var name = "Apple";
			var error = "Unit Test Failed";
			var result = Result<Domain.Products>.Failure(error);

			mediator.Setup(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(name).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<List.Query>(y => y.Name == name), It.IsAny<CancellationToken>()));
			var errorReturned = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
			errorReturned.Value.Should().Be(error);
		}

		/// <summary>
		/// Controller should return Ok response when it finds the product by its Id.
		/// Endpoint: GET /products/{id}
		/// </summary>
		[Fact]
		public void Get_ValidId_OkResponseWithProduct()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var product = SeedTestData.Products.Where(p => p.Id == Guid.Parse(id)).FirstOrDefault();
			var result = Result<Product>.Success(product);

			mediator.Setup(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
			var productReturned = okResult.Value.Should().BeAssignableTo<Product>().Subject;
			productReturned.Should().Be(product);
		}

		/// <summary>
		/// Controller should return Ok response with no product when it cannot find the products by Id.
		/// Endpoint: GET /products/{id}
		/// </summary>
		[Fact]
		public void Get_InvalidId_NotFoundResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";
			Product product = null;

			var result = Result<Domain.Product>.Success(product);

			mediator.Setup(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<NotFoundResult>().Subject;
		}

		/// <summary>
		/// Controller should return Ok response when it finds the product options for the given product id.
		/// Endpoint: GET /products/{id}/options
		/// </summary>
		[Fact]
		public void GetOptions_ValidId_OkResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptions = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(id)).ToList();
			var result = Result<ProductOptions>.Success(new ProductOptions { Items = productOptions });

			mediator.Setup(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOptions(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return not found response when it cannot find the products by Id.
		/// Endpoint: GET /products/{id}
		/// </summary>
		[Fact]
		public void GetOptions_InvalidId_NotFoundResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";

			Result<ProductOptions> result = null;
			
			mediator.Setup(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOptions(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}
	}
}
