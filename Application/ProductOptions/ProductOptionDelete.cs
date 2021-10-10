using Application.Helper;
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
	/// Mediator Pattern : Product Option Delete
	/// </summary>
	public class ProductOptionDelete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid ProductId { get; set; }
			public Guid Id { get; set; }
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
					_logger.LogInformation("Finding the correct product options");
					var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.Id == request.Id && p.ProductId == request.ProductId);

					if (productOption == null)
					{
						_logger.LogInformation("Unable to find the product option");
						return null;
					}

					_logger.LogInformation("Removing the product option from the data context");
					_context.Remove(productOption);

					_logger.LogInformation("Saving changes to DB");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Failed to delete the product option");
						return Result<Unit>.Failure("Failed to delete the product option");
					}

					_logger.LogInformation("Deleting the product option - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to delete the product option");
				}
			}
		}
	}
}
