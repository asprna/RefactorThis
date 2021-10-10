using Application.Helper;
using Domain;
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
	/// Mediator Pattern : Product Details
	/// </summary>
	public class Details
	{
		public class Query : IRequest<Result<Product>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Product>>
		{
			private readonly DataContext _context;
			private readonly ILogger<Handler> _logger;

			public Handler(DataContext context, ILogger<Handler> logger)
			{
				_context = context;
				_logger = logger;
			}

			public async Task<Result<Product>> Handle(Query request, CancellationToken cancellationToken)
			{
				try
				{
					_logger.LogInformation("Find the correct product");
					var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

					if (product == null)
					{
						_logger.LogInformation("Unable find the product");
						return null;
					}

					_logger.LogInformation("Getting product details - Success");
					return Result<Product>.Success(product);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Product>.Failure("The request failed");
				}
			}
		}
	}
}
