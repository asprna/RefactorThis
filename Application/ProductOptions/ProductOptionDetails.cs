using Application.Helper;
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

			public Handler(DataContext context, IMapper mapper)
			{
				_context = context;
				_mapper = mapper;
			}

			public async Task<Result<ProductOptionDTO>> Handle(Query request, CancellationToken cancellationToken)
			{
				var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);

				if (product == null)
				{
					return null;
				}

				var productOption = await _context.ProductOptions.FirstOrDefaultAsync(p => p.Id == request.Id && p.ProductId == request.ProductId);

				if (productOption == null)
				{
					return null;
				}

				var productOptionDTO = _mapper.Map<ProductOption, ProductOptionDTO>(productOption);

				return Result<ProductOptionDTO>.Success(productOptionDTO);
			}
		}
	}
}
