using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public record ProductOptionsDTO
	{
		public List<ProductOptionDTO> Items { get; set; }
	}
}
