using Application.Helper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProductList = Domain.Products;

namespace Application.Products
{
	/// <summary>
	/// Mediator Patter : Product List 
	/// </summary>
	public class List
	{
		public class Query : IRequest<Result<ProductList>> 
		{
			public string Name { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<ProductList>>
		{
			private readonly DataContext _context;
			private readonly ILogger<Handler> _logger;

			public Handler(DataContext context, ILogger<Handler> logger)
			{
				_context = context;
				_logger = logger;
			}

			public async Task<Result<ProductList>> Handle(Query request, CancellationToken cancellationToken)
			{
				var items = new List<Product>();

				if (!string.IsNullOrWhiteSpace(request.Name))
				{
					_logger.LogInformation("Find all products by its name");
					//Find all the products that matches the given Name
					items = await _context.Products.Where(p => p.Name.ToLower().Contains(request.Name.ToLower())).ToListAsync();
				}
				else
				{
					_logger.LogInformation("Find all products");
					//Find all the products
					items = await _context.Products.ToListAsync();
				}

				_logger.LogInformation("Adding all products to Products object");
				var products = new Domain.Products
				{
					Items = items
				};

				_logger.LogInformation("Get all products - Success");
				return Result<Domain.Products>.Success(products);
			}
		}
	}
}
