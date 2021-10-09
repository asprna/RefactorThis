using Application.Helper;
using Application.ProductOptions;
using AutoMapper;
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
	public class ProductOptionDetailsTest
	{
		private readonly ProductOptionDetails.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly IMapper _mapper;

		public ProductOptionDetailsTest()
		{
			var mockDb = new MockDb(_logger.Object);
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			_mapper = mappingConfig.CreateMapper();
			sut = new ProductOptionDetails.Handler(mockDb.GetTestDbContext(), _mapper);
		}

		/// <summary>
		/// The application should return the product option when the product id and option id  are valid
		/// </summary>
		[Fact]
		public void Handler_ValidProductIdAndOptionId_ReturnProductOptions()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABCC9";
			var productOption = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(productId) && p.Id == Guid.Parse(productOptionId)).FirstOrDefault();
			var productOptionDTO = _mapper.Map<ProductOption, ProductOptionDTO>(productOption);

			var request = new ProductOptionDetails.Query { ProductId = Guid.Parse(productId), Id = Guid.Parse(productOptionId) };

			var expectedResult = Result<ProductOptionDTO>.Success(productOptionDTO);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when the product id is invalid
		/// </summary>
		[Fact]
		public void Handler_InvalidProductIdAndValidOptionId_ReturnNull()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB133";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABCC9";
			var productOption = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(productId) && p.Id == Guid.Parse(productOptionId)).FirstOrDefault();

			var request = new ProductOptionDetails.Query { ProductId = Guid.Parse(productId), Id = Guid.Parse(productOptionId) };

			Result<ProductOption> expectedResult = null;

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when the product option id is invalid
		/// </summary>
		[Fact]
		public void Handler_ValidProductIdAndInvalidOptionId_ReturnNull()
		{
			//Arrange
			var productId = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptionId = "0643CCF0-AB00-4862-B3C5-40E2731ABC99";
			var productOption = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(productId) && p.Id == Guid.Parse(productOptionId)).FirstOrDefault();

			var request = new ProductOptionDetails.Query { ProductId = Guid.Parse(productId), Id = Guid.Parse(productOptionId) };

			Result<ProductOption> expectedResult = null;

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
	}
}
