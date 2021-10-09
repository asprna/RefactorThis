using Application.Helper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<Product>> Handle(Query request, CancellationToken cancellationToken)
			{
				//Find the products
				var product =  await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

				if(product == null)
				{
					return null;
				}

				return Result<Product>.Success(product);
			}
		}
	}
}
