using Application.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
	public class Delete
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Command, Result<Unit>>
		{
			private readonly DataContext _context;

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

				if (product == null)
				{
					return null;
				}

				_context.Remove(product);

				var result = await _context.SaveChangesAsync() > 0;

				if (!result)
				{
					return Result<Unit>.Failure("Failed to delete the product");
				}

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
