using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompositeModelBinding.Binders;
using CompositeModelBinding.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompositeModelBinding.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DemoController : ControllerBase
	{
		private readonly ILogger<DemoController> _logger;

		public DemoController(ILogger<DemoController> logger)
		{
			_logger = logger;
		}

		[HttpPut("regular/{id}")]
		public IActionResult Regular(int id, [FromBody] InferredDemoModel demoModel)
		{
			if (!demoModel.Id.HasValue)
			{
				_logger.LogInformation("Model missing ID (normal behavior)");
				demoModel.Id = id;
			}

			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}

		#region Using CompositeBindingSource
		[HttpGet("composite-compiled/{id}")]
		public IActionResult CompiledMultiSource([FromRouteAndQuery] DemoDynamicModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}

		[HttpGet("composite-dynamic/{id}")]
		public IActionResult DynamicMultiSource([FromMultiSource("DynamicRouteAndQuery", BindingSources = new[]
													{
														nameof(Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Path),
														nameof(Microsoft.AspNetCore.Mvc.ModelBinding.BindingSource.Query)
													})] DemoDynamicModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}
		#endregion Using CompositeBindingSource

		#region Using FromModel
		// these don't work

		[HttpPut("modelbinding/{id}")]
		public IActionResult WithModelBinding([FromModel] DemoModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}

		[HttpPut("modelbinding-nested/{id}")]
		public IActionResult WithNestedWithModelBinding([FromModel] DemoModelParent demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}
		#endregion Using FromModel

		#region Using FromBodyAndRoute
		[HttpPut("bodyroute/{id}")]
		public IActionResult WithModelBinderA([FromBodyAndRoute] DemoModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}

		[HttpPut("bodyroute-nested/{id}")]
		public IActionResult NestedWithModelBinder([FromBodyAndRoute] ParentModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}
		#endregion Using FromBodyAndRoute
	}
}
