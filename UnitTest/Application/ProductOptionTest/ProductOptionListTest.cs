using Application.Helper;
using Application.ProductOptions;
using Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
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
	public class ProductOptionListTest
	{
		private readonly ProductOptionList.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();

		public ProductOptionListTest()
		{
			var mockDb = new MockDb(_logger.Object);
			sut = new ProductOptionList.Handler(mockDb.GetTestDbContext());
		}

		/// <summary>
		/// The application should return all product option when the product id is valid
		/// </summary>
		[Fact]
		public void Handler_ValidProductId_ReturnAllProductOptions()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOption = new ProductOptions { Items = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(id)).ToList() };

			var request = new ProductOptionList.Query { ProductID = Guid.Parse(id) };

			var expectedResult = Result<ProductOptions>.Success(productOption);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when the product id is invalid
		/// </summary>
		[Fact]
		public void Handler_InValidProductId_ReturnNull()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";
			
			var request = new ProductOptionList.Query { ProductID = Guid.Parse(id) };

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeNull();
		}
	}
}
