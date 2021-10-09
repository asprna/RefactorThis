using Application.Helper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
	/// <summary>
	/// Mediator Pattern : Product Create
	/// </summary>
	public class Create
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Product Product { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.Product).SetValidator(new ProductValidator());
			}
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
				//Add the new product
				_context.Products.Add(request.Product);

				//Save changes to DB
				var result = await _context.SaveChangesAsync() > 0;

				if (!result) return Result<Unit>.Failure("Failed to create product");

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
