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

namespace Application.ProductOptions
{
	/// <summary>
	/// Mediator Pattern : Product Option Edit
	/// </summary>
	public class ProductOptionEdit
	{
		public class Command : IRequest<Result<Unit>>
		{
			public ProductOption ProductOption { get; set; }

			public Guid Id { get; set; }
		}

		public class CommandValidator : AbstractValidator<Command>
		{
			public CommandValidator()
			{
				RuleFor(x => x.ProductOption).SetValidator(new ProductOptionValidator());
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
				//Find the product option
				var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.ProductId == request.ProductOption.ProductId && p.Id == request.Id);

				if (productOption == null)
				{
					return null;
				}

				//Map product option with the prduct option in the request
				_mapper.Map(request.ProductOption, productOption);

				//Save changes to DB
				var result = await _context.SaveChangesAsync() > 0;

				if (!result)
				{
					return Result<Unit>.Failure("Failed to edit product option");
				}

				return Result<Unit>.Success(Unit.Value);
			}
		}
	}
}
