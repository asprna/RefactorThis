using Application.Helper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ProductOptions
{
	public class ProductOptionDetails
	{
		public class Query : IRequest<Result<ProductOption>>
		{
			public Guid ProductId { get; set; }
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<ProductOption>>
		{
			private readonly DataContext _context;

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<ProductOption>> Handle(Query request, CancellationToken cancellationToken)
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

				if (product == null)
				{
					return null;
				}

				var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.Id == request.Id && p.ProductId == request.ProductId);

				if (productOption == null)
				{
					return null;
				}

				return Result<ProductOption>.Success(productOption);
			}
		}
	}
}
