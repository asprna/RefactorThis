using Application.Helper;
using Application.Products;
using AutoMapper;
using Domain;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
	public class EditTest
	{
		private readonly Edit.Handler sut;
		private readonly Details.Handler sutDetail;
		private readonly Mock<ILogger<MockDb>> _logger = new Mock<ILogger<MockDb>>();
		private readonly Mock<DataContext> _context = new Mock<DataContext>();
		//private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

		public EditTest()
		{
			var mockDb = new MockDb(_logger.Object);
			var mappingConfig = new MapperConfiguration(mc => 
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();

			sut = new Edit.Handler(mockDb.GetTestDbContext(), mapper);
			sutDetail = new Details.Handler(mockDb.GetTestDbContext());
		}

		/// <summary>
		/// The application should update the product correctly when the product is valid.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Handler_ValidProduct_EditSuccess()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var newProduct = new Product
			{
				Id = product.Id,
				Name = product.Name,
				Description = "Updated Description",
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};

			var request = new Edit.Command { Product = newProduct, Id = Guid.Parse(id) };
			var requestProduct = new Details.Query { Id = product.Id };

			var expectedResult = Result<Unit>.Success(Unit.Value);

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;
			var updatedProduct = sutDetail.Handle(requestProduct, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
			updatedProduct.Value.Should().Be(product);
		}

		/// <summary>
		/// The application should return error when it fails to update the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Handler_FailedToUpdate_ErrorReturned()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var request = new Edit.Command { Product = product, Id = Guid.Parse(id) };
			var requestProduct = new Details.Query { Id = product.Id };

			var expectedResult = Result<Unit>.Failure("Failed to update product");

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeEquivalentTo(expectedResult);
		}

		/// <summary>
		/// The application should return null when it fails to find the product.
		/// Endpoint: POST /products
		/// </summary>
		[Fact]
		public void Handler_ProductIdIsWrong_NotFoundResponse()
		{
			//Arrange
			var id = "DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3";
			var product = SeedTestData.Products.Where(x => x.Id == Guid.Parse(id)).FirstOrDefault();

			var newProduct = new Product
			{
				Id = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFE33"),
				Name = product.Name,
				Description = product.Description,
				DeliveryPrice = product.DeliveryPrice,
				Price = product.Price
			};


			var request = new Edit.Command { Product = newProduct, Id = newProduct.Id };

			//Act
			var result = sut.Handle(request, CancellationToken.None).Result;

			//Assert
			result.Should().BeNull();
		}
	}
}
