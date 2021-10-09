using Application.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

			public Handler(DataContext context)
			{
				_context = context;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				//Find the product option 
				var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.Id == request.Id && p.ProductId == request.ProductId);

				if (productOption == null)
				{
					return null;
				}

				//Remove the product option from the data context
				_context.Remove(productOption);

				//Save changes to DB
				var result = await _context.SaveChangesAsync() > 0;

				if (!result)
				{
					return Result<Unit>.Failure("Failed to delete the product option");
				}

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
