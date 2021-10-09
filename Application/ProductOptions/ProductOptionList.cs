using Application.Helper;
using Application.Products;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

			public Handler(DataContext context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}

			public async Task<Result<Domain.ProductOptionsDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				//Find the product
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductID);

				if (product == null)
				{
					return null;
				}

				//Find the product options for the given product
				var options = await _context.ProductOptions.Where(p => p.ProductId == request.ProductID).ToListAsync();

				//Map Product Option to Product option DTO
				List<ProductOptionDTO> productOptionDTOs = _mapper.Map<List<ProductOption>, List<ProductOptionDTO>>(options);

				var productOptions = new Domain.ProductOptionsDTO
				{
					Items = productOptionDTOs
				};

				return Result<Domain.ProductOptionsDTO>.Success(productOptions);
			}
		}
	}
}
