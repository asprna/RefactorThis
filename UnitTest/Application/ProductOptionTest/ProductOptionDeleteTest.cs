using Application.Helper;
using Application.ProductOptions;
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
	public class ProductOptionDeleteTest
	{
		private readonly ProductOptionDelete.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly Mock<DataContext> _context = new Mock<DataContext>();

		public ProductOptionDeleteTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new ProductOptionDelete.Handler(mockDb.GetTestDbContext());
		}

		/// <summary>
		/// The application should Delete the product option correctly when the product id and option id are valid.
		/// </summary>
		[Fact]
		public void Handler_ValidProductIdAndOptionID_DeleteSuccess()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03";

			var request = new ProductOptionDelete.Command { ProductId = Guid.Parse(productId),  Id = Guid.Parse(id) };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when it fails to find the product option.
		/// </summary>
		[Fact]
		public void Handler_InvalidProductIdAndValidOptionID_ReturnNull()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F03";

			var request = new ProductOptionDelete.Command { ProductId = Guid.Parse(productId), Id = Guid.Parse(id) };

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeNull();
		}

		/// <summary>
		/// The application should return null when it fails to find the product option.
		/// </summary>
		[Fact]
		public void Handler_ValidProductIdAndInvalidOptionID_ReturnNull()
		{
			//Arrange
			var productId = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var id = "9AE6F477-A010-4EC9-B6A8-92A85D6C5F33";

			var request = new ProductOptionDelete.Command { ProductId = Guid.Parse(productId), Id = Guid.Parse(id) };

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeNull();
		}
	}
}
