using Application.Helper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
	/// <summary>
	/// Mediator Pattern : Product Create
	/// </summary>
	public class Create
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Product Product { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.Product).SetValidator(new ProductValidator());
			}
		}

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
					_logger.LogInformation("Adding a new product");
					_context.Products.Add(request.Product);

					_logger.LogInformation("Saving DB changes");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Unable to save changes to DB");
						return Result<Unit>.Failure("Failed to create product");
					}

					_logger.LogInformation("Create a new product - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to create product");
				}
			}
		}
	}
}
