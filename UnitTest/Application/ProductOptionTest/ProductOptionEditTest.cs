using Application.Helper;
using Application.ProductOptions;
using AutoMapper;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using UnitTest.Helper;
using Xunit;

namespace UnitTest.Application.ProductOptionTest
{
	public class ProductOptionEditTest
	{
		private readonly ProductOptionEdit.Handler sut;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		public ProductOptionEditTest()
		{
			var mockDb = new MockDb(_logger.Object);
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();

			sut = new ProductOptionEdit.Handler(mockDb.GetTestDbContext(), mapper);
		}

		/// <summary>
		/// The application should update the product option correctly when the product is valid.
		/// </summary>
		[Fact]
		public void Handler_ValidProductOption_EditSuccess()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = "Updated Description",
				ProductId = productOption.ProductId
			};

			var request = new ProductOptionEdit.Command { ProductOption  = newProductOption, Id = newProductOption.Id };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return an error when the product option is not found.
		/// </summary>
		[Fact]
		public void Handler_InvalidProductOption_ReturnNull()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = "Updated Description",
				ProductId = productOption.ProductId
			};

			var request = new ProductOptionEdit.Command { ProductOption = newProductOption, Id = Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABC99") };

			Result<Unit> expectedResult = null;

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return bad request when failed to update the product option.
		/// </summary>
		[Fact]
		public void Handler_FailedToUpdate_ReturnError()
		{
			//Arrange
			var productOption = SeedTestData.ProductOptions.Where(x => x.Id == Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9") && x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3")).FirstOrDefault();

			var newProductOption = new ProductOption
			{
				Id = productOption.Id,
				Name = productOption.Name,
				Description = productOption.Description,
				ProductId = productOption.ProductId
			};

			var request = new ProductOptionEdit.Command { ProductOption = newProductOption, Id = newProductOption.Id };

			var expectedResult = Result<Unit>.Failure("Failed to edit product option");

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}
	}
}
