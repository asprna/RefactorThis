using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
	public record Products
	{
		public List<Product> Items { get; set; }
	}
}
