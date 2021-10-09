using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products
{
	/// <summary>
	/// Product Validator
	/// </summary>
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Description).NotEmpty();
			RuleFor(x => x.Price).NotEmpty();
			RuleFor(x => x.DeliveryPrice).NotEmpty();
		}
	}
}
