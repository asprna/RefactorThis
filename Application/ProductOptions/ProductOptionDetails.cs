using Application.Helper;
using AutoMapper;
using Domain;
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
	/// Mediator Pattern = Product Option Details
	/// </summary>
	public class ProductOptionDetails
	{
		public class Query : IRequest<Result<ProductOptionDTO>>
		{
			public Guid ProductId { get; set; }
			public Guid Id { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<ProductOptionDTO>>
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

			public async Task<Result<ProductOptionDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				try
				{
					_logger.LogInformation("Checking if the product exists");
					var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

					if (product == null)
					{
						_logger.LogInformation("Unable find the product details");
						return null;
					}

					_logger.LogInformation("Finding the product option");
					var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.Id == request.Id && p.ProductId == request.ProductId);

					if (productOption == null)
					{
						_logger.LogInformation("Unable find the product option");
						return null;
					}

					_logger.LogInformation("Mapping DB Contest product option to product option DTO");
					var productOptionDTO = _mapper.Map<ProductOption, ProductOptionDTO>(productOption);

					_logger.LogInformation("Getting the product option - Success");
					return Result<ProductOptionDTO>.Success(productOptionDTO);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Unhandle error occured");
					return Result<ProductOptionDTO>.Failure("The request failed");
				}
			}
		}
	}
}
