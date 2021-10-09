using System;
using System.Collections.Generic;

namespace Domain
{
	public record ProductOptions
	{
		public List<ProductOption> Items { get; set; }
	}
}
