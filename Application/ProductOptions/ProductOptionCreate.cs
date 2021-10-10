using Application.Helper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
			private readonly ILogger<Handler> _logger;

			public Handler(DataContext context, ILogger<Handler> logger)
			{
				_context = context;
				_logger = logger;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				try
				{
					_logger.LogInformation("Checking if the product exists");
					var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

					if (product == null)
					{
						_logger.LogInformation("Product does not exist");
						return null;
					}

					//Check if product id is correct in the product option object.
					if (request.ProductId != request.ProductOption.ProductId)
					{
						_logger.LogInformation("Requested product id should be same as the product id of the product option");
						return Result<Unit>.Failure("Failed to create product option");
					}

					_logger.LogInformation("Adding the product option to the product");
					_context.ProductOptions.Add(request.ProductOption);

					_logger.LogInformation("Saving changes to DB");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Failed to create product option");
						
					}

					_logger.LogInformation("Adding a new product option - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to create product option");
				}
				
			}
		}
	}
}
