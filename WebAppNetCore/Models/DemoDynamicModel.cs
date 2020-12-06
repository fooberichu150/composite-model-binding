using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAppNetCore.Models
{
	public class DemoDynamicModel
	{
		[FromRoute]
		public int? Id { get; set; }
		[FromQuery]
		public string Value1 { get; set; }
		[FromQuery]
		public bool Value2 { get; set; }
	}
}
