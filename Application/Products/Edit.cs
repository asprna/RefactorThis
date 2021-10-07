using Application.Helper;
using AutoMapper;
using Domain;
using FluentValidation;
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
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

				if (product == null)
				{
					return null;
				}

				_mapper.Map(request.Product, product);

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
