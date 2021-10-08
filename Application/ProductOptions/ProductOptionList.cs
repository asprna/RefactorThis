using Application.Helper;
using Application.Products;
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
	public class ProductOptionList
	{
		public class Query : IRequest<Result<Domain.ProductOptions>>
		{
			public Guid ProductID { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Domain.ProductOptions>>
		{
			private readonly DataContext _context;

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<Domain.ProductOptions>> Handle(Query request, CancellationToken cancellationToken)
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductID);

				if (product == null)
				{
					return null;
				}

				var options = await _context.ProductOptions.Where(p => p.ProductId == request.ProductID).ToListAsync();

				var productOptions = new Domain.ProductOptions
				{
					Items = options
				};

				return Result<Domain.ProductOptions>.Success(productOptions);
			}
		}
	}
}
