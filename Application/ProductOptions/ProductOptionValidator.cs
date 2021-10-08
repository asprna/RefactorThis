using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ProductOptions
{
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
