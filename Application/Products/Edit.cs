using Application.Helper;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
			private readonly ILogger<Handler> _logger;

			public Handler(DataContext context, IMapper mapper, ILogger<Handler> logger)
			{
				_context = context;
				_mapper = mapper;
				_logger = logger;
			}

			public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
			{
				try
				{
					_logger.LogInformation("Find the correct product");
					var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

					if (product == null)
					{
						_logger.LogInformation("Unable find the product");
						return null;
					}

					_logger.LogInformation("Mapping the product changes from the request to product context");
					_mapper.Map(request.Product, product);

					_logger.LogInformation("Saving changes to DB");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Failed to update product");
						return Result<Unit>.Failure("Failed to update product");
					}

					_logger.LogInformation("Editing product details - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to update product");
				}
			}
		}
	}
}
