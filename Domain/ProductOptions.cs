using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public record ProductOptions
	{
		public List<ProductOption> Items { get; set; }
	}
}
