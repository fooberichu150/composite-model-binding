using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppNetCore.Models
{
	public class ParentModel
	{
		public int? Id { get; set; }

		public string Blah { get; set; }

		public NestedModel Nested { get; set; }
	}

	public class NestedModel
	{
		public string Value1 { get; set; }
		public bool Value2 { get; set; }
	}
}
