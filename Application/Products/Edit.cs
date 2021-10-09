using Application.Helper;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Products
{
	/// <summary>
	/// Mediator Pattern : Product Edit
	/// </summary>
	public class Edit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public Product Product { get; set; }

			public Guid Id { get; set; }
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
			private readonly IMapper _mapper;

			public Handler(DataContext context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				//Find the product
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

				if (product == null)
				{
					return null;
				}

				//Map product changes from the request to product
				_mapper.Map(request.Product, product);

				//Save product to DB
				var result = await _context.SaveChangesAsync() > 0;

				if (!result)
				{
					return Result<Unit>.Failure("Failed to update product");
				}
				
				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
