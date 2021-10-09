using Application.Helper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<ProductList>> Handle(Query request, CancellationToken cancellationToken)
			{
				var items = new List<Product>();

				if (!string.IsNullOrWhiteSpace(request.Name))
				{
					//Find all the products that matches the given Name
					items = await _context.Products.Where(p => p.Name.ToLower().Contains(request.Name.ToLower())).ToListAsync();
				}
				else
				{
					//Find all the products
					items = await _context.Products.ToListAsync();
				}

				var products = new Domain.Products
				{
					Items = items
				};

				return Result<Domain.Products>.Success(products);
			}
		}
	}
}
