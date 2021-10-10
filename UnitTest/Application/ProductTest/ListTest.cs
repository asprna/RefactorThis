using Application.Helper;
using Application.Products;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.Application.ProductTest
{
	public class ListTest
	{
		private readonly List.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly Mock<ILogger<List.Handler>> _loggerHandler = new Mock<ILogger<List.Handler>>();
		public ListTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new List.Handler(mockDb.GetTestDbContext(), _loggerHandler.Object);
		}

		/// <summary>
		/// The application should return all product when the name is null
		/// </summary>
		[Fact]
		public void Handler_WhenNameIsNull_ReturnAllProducts()
		{
			//Arrange
			var products = new Products
			{
				Items = SeedTestData.Products
			};

			var request = new List.Query { Name = null };

			var expectedResult = Result<Domain.Products>.Success(products);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.IsSuccess.Should().BeTrue();
			result.Value.Items.Should().Contain(products.Items);
		}

		/// <summary>
		/// The application should return all the specific product when the name is not null
		/// </summary>
		[Fact]
		public void Handler_WhenNameIsNotNull_ReturnFilteredProducts()
		{
			//Arrange
			var name = "Apple";
			var products = new Products
			{
				Items = SeedTestData.Products.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList()
			};

			var request = new List.Query { Name = name };

			var expectedResult = Result<Domain.Products>.Success(products);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.IsSuccess.Should().BeTrue();
			result.Value.Items.Should().Contain(products.Items);
		}

		/// <summary>
		/// The application should not return products when the name is not valid
		/// </summary>
		[Fact]
		public void Handler_WhenNameIsInvalid_ReturnNoProducts()
		{
			//Arrange
			var name = "NoProduct";
			var products = new Products
			{
				Items = SeedTestData.Products.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList()
			};

			var request = new List.Query { Name = name };

			var expectedResult = Result<Domain.Products>.Success(products);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.IsSuccess.Should().BeTrue();
			result.Value.Items.Count.Should().Be(products.Items.Count());
		}
	}
}
