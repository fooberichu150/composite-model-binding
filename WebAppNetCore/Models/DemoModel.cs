using Microsoft.AspNetCore.Mvc;

namespace WebAppNetCore.Models
{
	public class DemoModel
	{
		[FromRoute]
		public int? Id { get; set; }

		public string Value1 { get; set; }
		public bool Value2 { get; set; }
	}
}
