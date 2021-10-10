using Application.Helper;
using Application.Products;
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
	public class DeleteTest
	{
		private readonly Delete.Handler sut;
		private readonly Details.Handler sutDetail;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();

		public DeleteTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new Delete.Handler(mockDb.GetTestDbContext());
		}

		/// <summary>
		/// The application should Delete the product correctly when the product is valid.
		/// </summary>
		[Fact]
		public void Handler_ValidProduct_EditSuccess()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			
			var request = new Delete.Command { Id = Guid.Parse(id) };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when it fails to find the product.
		/// </summary>
		[Fact]
		public void Handler_ProductIdIsWrong_ReturnNull()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33";
			
			var request = new Delete.Command { Id = Guid.Parse(id) };

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeNull();
		}
	}
}
