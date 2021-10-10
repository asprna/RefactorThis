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
					_logger.LogInformation("Checking if the product options exists");
					var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.ProductId == request.ProductOption.ProductId && p.Id == request.Id);

					if (productOption == null)
					{
						_logger.LogInformation("Unable to find the product");
						return null;
					}

					_logger.LogInformation("Mapping the product option in the request to the product option in the datacontext");
					_mapper.Map(request.ProductOption, productOption);

					_logger.LogInformation("Saving changes to DB");
					var result = await _context.SaveChangesAsync() > 0;

					if (!result)
					{
						_logger.LogInformation("Failed to edit product option");
						return Result<Unit>.Failure("Failed to edit product option");
					}

					_logger.LogInformation("Editing the product option - Success");
					return Result<Unit>.Success(Unit.Value);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<Unit>.Failure("Failed to edit product option");
				}
				
			}
		}
	}
}
