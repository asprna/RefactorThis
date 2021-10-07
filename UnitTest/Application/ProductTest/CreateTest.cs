using Application.Helper;
using Application.Products;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

		public CreateTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new Create.Handler(mockDb.GetTestDbContext());
			sutDetail = new Details.Handler(mockDb.GetTestDbContext());
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
			var requestProduct = new Details.Query { Id = product.Id };

			var expectedResult = Result<Product>.Success(product);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;
			var newProduct = sutDetail.Handle(requestProduct, CancellationToken.None).Result;

			//Assert
			result.IsSuccess.Should().BeTrue();
			newProduct.Value.Should().Be(product);
		}

		/// <summary>
		/// The application should aerror when failed to add the product
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
			var sutDataContext = new Create.Handler(_context.Object);
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
