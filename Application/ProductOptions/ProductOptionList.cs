using Application.Helper;
using Application.Products;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ProductOptions
{
	/// <summary>
	/// Mediator Pattern : Product Option List
	/// </summary>
	public class ProductOptionList
	{
		public class Query : IRequest<Result<Domain.ProductOptionsDTO>>
		{
			public Guid ProductID { get; set; }
		}

		public class Handler : IRequestHandler<Query, Result<Domain.ProductOptionsDTO>>
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

			public async Task<Result<Domain.ProductOptionsDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				_logger.LogInformation("Checking if the product exists");
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductID);

				if (product == null)
				{
					_logger.LogInformation("Unable to find the product");
					return null;
				}

				_logger.LogInformation("Checking if the product option exists for the given product id");
				var options = await _context.ProductOptions.Where(p => p.ProductId == request.ProductID).ToListAsync();

				_logger.LogInformation("Mapping the product option in the request to Product option n the DB");
				List<ProductOptionDTO> productOptionDTOs = _mapper.Map<List<ProductOption>, List<ProductOptionDTO>>(options);

				var productOptions = new Domain.ProductOptionsDTO
				{
					Items = productOptionDTOs
				};

				_logger.LogInformation("Getting the product option - Success");
				return Result<Domain.ProductOptionsDTO>.Success(productOptions);
			}
		}
	}
}
