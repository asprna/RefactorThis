using Application.Helper;
using Application.ProductOptions;
using Domain;
using FluentAssertions;
using MediatR;
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

namespace UnitTest.Application.ProductOptionTest
{
	public class ProductOptionCreateTest
	{
		private readonly ProductOptionCreate.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly Mock<DataContext> _context = new Mock<DataContext>();

		public ProductOptionCreateTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new ProductOptionCreate.Handler(mockDb.GetTestDbContext());
		}

		/// <summary>
		/// The application should create the product option when the product id and option are valid
		/// </summary>
		[Fact]
		public void Handler_ValidProductIdAndOptionId_ReturnProductOptions()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var request = new ProductOptionCreate.Command { ProductId = newProductOption.ProductId, ProductOption = newProductOption };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return Null when the product id is invalid
		/// </summary>
		[Fact]
		public void Handler_InvalidProductIdAndOptionId_ReturnNull()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var request = new ProductOptionCreate.Command { ProductId = newProductOption.ProductId, ProductOption = newProductOption };

			Result<Unit> expectedResult = null;

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return an error when the product ids are mismatched
		/// </summary>
		[Fact]
		public void Handler_ProductIdsMismatched_ReturnError()
		{
			//Arrange
			var newProductOption = new ProductOption
			{
				Id = Guid.NewGuid(),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB133"),
				Description = "Dummy Option",
				Name = "Dummy Name"
			};

			var request = new ProductOptionCreate.Command { ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), ProductOption = newProductOption };

			var expectedResult = Result<Unit>.Failure("Failed to create product option");

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
	}
}
