using Microsoft.AspNetCore.Mvc;

namespace CompositeModelBinding.Models
{
	public class DemoModel
	{
		[FromRoute]
		public int? Id { get; set; }

		public string Value1 { get; set; }
		public bool Value2 { get; set; }
	}

	public class DemoModelParent
	{
		[FromRoute]
		public int? Id { get; set; }

		public string Blah { get; set; }

		[FromBody]
		[BindProperty(Name = "")]
		public NestedModel Nested { get; set; }
	}
}
