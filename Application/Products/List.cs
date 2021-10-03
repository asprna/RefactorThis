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

namespace Application.Products
{
	public class List
	{
		public class Query : IRequest<List<Product>> { }

		public class Handler : IRequestHandler<Query, List<Product>>
		{
			private readonly DataContext _context;

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<List<Product>> Handle(Query request, CancellationToken cancellationToken)
			{
				return await _context.Products.ToListAsync();
			}
		}
	}
}
