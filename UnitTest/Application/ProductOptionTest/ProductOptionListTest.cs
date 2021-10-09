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
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.Application.ProductOptionTest
{
	public class ProductOptionListTest
	{
		private readonly ProductOptionList.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly IMapper _mapper;

		public ProductOptionListTest()
		{
			var mockDb = new MockDb(_logger.Object);
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			_mapper = mappingConfig.CreateMapper();
			sut = new ProductOptionList.Handler(mockDb.GetTestDbContext(), _mapper);
		}

		/// <summary>
		/// The application should return all product option when the product id is valid
		/// </summary>
		[Fact]
		public void Handler_ValidProductId_ReturnAllProductOptions()
		{
			//Arrange
			var id = "8F2E9176-35EE-4F0A-AE55-83023D2DB1A3";
			var productOptions = SeedTestData.ProductOptions.Where(p => p.ProductId == Guid.Parse(id)).ToList();
			var productOptionsDTO = _mapper.Map<List<ProductOption>, List<ProductOptionDTO>>(productOptions);
			var productOption = new ProductOptionsDTO { Items = productOptionsDTO };

			var request = new ProductOptionList.Query { ProductID = Guid.Parse(id) };

			var expectedResult = Result<ProductOptionsDTO>.Success(productOption);

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
