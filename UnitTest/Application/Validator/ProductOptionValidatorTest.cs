using Application.ProductOptions;
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
	/// <summary>
	/// Product option validator
	/// </summary>
	public class ProductOptionValidatorTest
	{
		private readonly ProductOptionValidator _productOptionValidator;

		public ProductOptionValidatorTest()
		{
			_productOptionValidator = new ProductOptionValidator();
		}

		[Fact]
		public void Product_AllFieldsRequired()
		{
			//Arrange
			var productOption = new ProductOption { Name = null };

			//Act
			var result = _productOptionValidator.TestValidate(productOption);

			//Assert
			result.ShouldHaveValidationErrorFor(p => p.Id);
			result.ShouldHaveValidationErrorFor(p => p.ProductId);
			result.ShouldHaveValidationErrorFor(p => p.Description);
			result.ShouldHaveValidationErrorFor(p => p.Name);
		}
	}
}
