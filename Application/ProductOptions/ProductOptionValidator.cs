using Domain;
using FluentValidation;

namespace Application.ProductOptions
{
	/// <summary>
	/// Product Option Validator
	/// </summary>
	public class ProductOptionValidator : AbstractValidator<ProductOption>
	{
		public ProductOptionValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}
