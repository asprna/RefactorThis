using API.Controllers;
using Application.Helper;
using Application.ProductOptions;
using Application.Products;
using AutoMapper;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.API
{
	public class ProductControllerGetTest
	{
		private readonly ProductsController sut;
		private readonly Mock<IMediator> mediator = new Mock<IMediator>();
		private readonly IMapper _mapper;

		public ProductControllerGetTest()
		{
			sut = new ProductsController(mediator.Object);

			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			_mapper = mappingConfig.CreateMapper();
		}

		/// <summary>
		/// Controller should return Ok response when it finds the products.
		/// </summary>
		[Fact]
		public void Get_NoParameter_OkResponse()
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
		}

		/// <summary>
		/// Controller should return Ok response with no products when it cannot find the products.
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
		/// </summary>
		[Fact]
		public void Get_FilterByInvalidName_OkResponseWithNoProduct()
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
		/// Controller should return Ok response when it finds the product by its Id.
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
		/// </summary>
		[Fact]
		public void Get_InvalidId_NotFoundResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";
			Product product = null;

			Result<Domain.Product> result = null;

			mediator.Setup(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.Get(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<Details.Query>(y => y.Id == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			var okResult = actionResult.Should().BeOfType<NotFoundResult>().Subject;
		}

		/// <summary>
		/// Controller should return Ok response when it finds the product options for the given product id.
		/// </summary>
		[Fact]
		public void GetOptions_ValidId_OkResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptions = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(id)).ToList();
			var productOptionsDTO = _mapper.Map<List<ProductOption>, List<ProductOptionDTO>>(productOptions);
			var result = Result<ProductOptionsDTO>.Success(new ProductOptionsDTO { Items = productOptionsDTO });

			mediator.Setup(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOptions(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return not found response when it cannot find the products by Id.
		/// </summary>
		[Fact]
		public void GetOptions_InvalidId_NotFoundResponse()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";

			Result<ProductOptionsDTO> result = null;
			
			mediator.Setup(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOptions(Guid.Parse(id)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionList.Query>(y => y.ProductID == Guid.Parse(id)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}

		/// <summary>
		/// Controller should return Ok response when it finds the product option details for the given product id and option id.
		/// </summary>
		[Fact]
		public void GetOptions_ValidPtoductIdAndValidOptionId_OkResponse()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABCC9";
			var productOption = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(productId) && p.Id == Guid.Parse(productOptionId)).FirstOrDefault();
			var productOptionDTO = _mapper.Map<ProductOption, ProductOptionDTO>(productOption);
			var result = Result<ProductOptionDTO>.Success(productOptionDTO);

			mediator.Setup(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOption(Guid.Parse(productId), Guid.Parse(productOptionId)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<OkObjectResult>();
		}

		/// <summary>
		/// Controller should return Not Found response when the given product id is invalid.
		/// </summary>
		[Fact]
		public void GetOptions_InvalidPtoductIdAndValidOptionId_NotFoundResponse()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABCC9";
			Result<ProductOptionDTO> result = null;

			mediator.Setup(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOption(Guid.Parse(productId), Guid.Parse(productOptionId)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}

		/// <summary>
		/// Controller should return Not Found response when the given product option id is invalid.
		/// </summary>
		[Fact]
		public void GetOptions_ValidPtoductIdAndInvalidOptionId_NotFoundResponse()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABC99";
			Result<ProductOptionDTO> result = null;

			mediator.Setup(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>())).ReturnsAsync(result);

			//Act
			var actionResult = sut.GetOption(Guid.Parse(productId), Guid.Parse(productOptionId)).Result;

			//Assert
			mediator.Verify(x => x.Send(It.Is<ProductOptionDetails.Query>(y => y.ProductId == Guid.Parse(productId) && y.Id == Guid.Parse(productOptionId)), It.IsAny<CancellationToken>()));
			actionResult.Should().BeOfType<NotFoundResult>();
		}
	}
}
