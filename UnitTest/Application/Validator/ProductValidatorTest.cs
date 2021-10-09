using Application.Products;
using Domain;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Application.Validator
{
	public class ProductValidatorTest
	{
		private readonly ProductValidator _productValidator;

		public ProductValidatorTest()
		{
			_productValidator = new ProductValidator();
		}

		[Fact]
		public void Product_AllFieldsRequired()
		{
			//Arrange
			var product = new Product { Name = null };

			//Act
			var result = _productValidator.TestValidate(product);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Id);
			result.ShouldHaveValidationErrorFor(p => p.Name);
			result.ShouldHaveValidationErrorFor(p => p.Description);
			result.ShouldHaveValidationErrorFor(p => p.Price);
			result.ShouldHaveValidationErrorFor(p => p.DeliveryPrice);

		}
	}
}
