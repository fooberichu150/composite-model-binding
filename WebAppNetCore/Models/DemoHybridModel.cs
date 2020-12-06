using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppNetCore.Models
{
	public class DemoHybridModel
	{
		public int? Id { get; set; }

		public string Blah { get; set; }

		public NestedModel Nested { get; set; }
	}
}
