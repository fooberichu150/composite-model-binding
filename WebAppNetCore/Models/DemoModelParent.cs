using Microsoft.AspNetCore.Mvc;

namespace WebAppNetCore.Models
{
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
