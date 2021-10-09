using System;
using System.Collections.Generic;

namespace Domain
{
	public record Products
	{
		public List<Product> Items { get; set; }
	}
}
