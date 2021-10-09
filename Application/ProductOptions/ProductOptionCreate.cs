using Application.Helper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ProductOptions
{
	/// <summary>
	/// Mediator Pattern : Product Option Create
	/// </summary>
	public class ProductOptionCreate
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid ProductId { get; set; }
			public ProductOption ProductOption { get; set; }
		}

		/// <summary>
		/// Product Option Validator.
		/// </summary>
		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.ProductOption).SetValidator(new ProductOptionValidator());
			}
		}

		/// <summary>
		/// Product Option create handler.
		/// </summary>
		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext _context;

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				//Find the product
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

				if (product == null)
				{
					return null;
				}

				//Check if product id is correct in the product option object.
				if (request.ProductId != request.ProductOption.ProductId)
				{
					return Result<Unit>.Failure("Failed to create product option");
				}

				//Add the product option
				_context.ProductOptions.Add(request.ProductOption);

				//Save changes to DB
				var result = await _context.SaveChangesAsync() > 0;

				//Validate the DB action
				if (!result) return Result<Unit>.Failure("Failed to create product option");

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
