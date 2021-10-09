using AutoMapper;
using Domain;

namespace Application.Helper
{
	/// <summary>
	/// AutoMapper Profiler
	/// </summary>
	public class MappingProfile : Profile
	{
		/// <summary>
		/// AutoMapper configuration.
		/// </summary>
		public MappingProfile()
		{
			CreateMap<Product, Product>();
			CreateMap<ProductOption, ProductOption>();
			CreateMap<ProductOption, ProductOptionDTO>();
		}
	}
}
