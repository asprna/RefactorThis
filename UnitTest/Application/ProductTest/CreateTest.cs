using Application.Helper;
using Application.Products;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence;
using System;
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.Application.ProductTest
{
	public class CreateTest
	{
		private readonly Create.Handler sut;
		private readonly Details.Handler sutDetail;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly Mock<DataContext> _context = new Mock<DataContext>();
		private readonly Mock<ILogger<Create.Handler>> _loggerCreateHandler = new Mock<ILogger<Create.Handler>>();
		private readonly Mock<ILogger<Details.Handler>> _loggerDetailsHandler = new Mock<ILogger<Details.Handler>>();

		public CreateTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new Create.Handler(mockDb.GetTestDbContext(), _loggerCreateHandler.Object);
			sutDetail = new Details.Handler(mockDb.GetTestDbContext(), _loggerDetailsHandler.Object);
		}

		/// <summary>
		/// The application should add the product to db when the product is valid
		/// </summary>
		[Fact]
		public void Handler_ValidProduct_InsertSuccess()
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

			var request = new Create.Command { Product = product };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return an error when failed to add the product
		/// </summary>
		[Fact]
		public void Handler_InsertFail_ErrorReturn()
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

			var request = new Create.Command { Product = product };

			//Mocking the Datacontext
			var sutDataContext = new Create.Handler(_context.Object, _loggerCreateHandler.Object);
			_context.Setup(x => x.Products.Add(It.IsAny<Product>()));
			_context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(-1);

			//Act
			var result = sutDataContext.Handle(request, CancellationToken.None).Result;

			//Assert
			result.IsSuccess.Should().BeFalse();
			result.Error.Should().Be("Failed to create product");
		}

	}
}
