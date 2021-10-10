using Application.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
	/// <summary>
	/// Mediator Pattern : Product Delete
	/// </summary>
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
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
					_logger.LogInformation("Find the correct product by its product ID");
					var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

					if (product == null)
					{
						_logger.LogInformation("Product not found");
						return null;
					}

					_logger.LogInformation("Removing the product from the datacontext");
					_context.Remove(product);

					_logger.LogInformation("Saving changes to DB");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Unable to delete the product");
						return Result<Unit>.Failure("Failed to delete the product");
					}

					_logger.LogInformation("Product deletion - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to delete the product");
				}
			}
		}
	}
}
