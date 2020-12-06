using HybridModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAppNetCore.Binders;
using WebAppNetCore.Models;

namespace WebAppNetCore.Controllers
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

		[HttpGet("regular-b/{id}")]
		public IActionResult RegularB(int id, DemoDynamicModel demoModel)
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
		[HttpGet("modelbinding/{id}")]
		public IActionResult WithModelBinding([FromModel]DemoDynamicModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}

		// these don't work for the BODY bindings
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

		#region Using HybridModelBinding
		// intentionally making id nullable to test FluentValidator rule
		[HttpPut("hybrid-model/{id?}")]
		public IActionResult HybridModel([FromHybrid]DemoHybridModel demoModel)
		{
			_logger.LogInformation("Incoming model: {0}", System.Text.Json.JsonSerializer.Serialize(demoModel));
			return StatusCode(StatusCodes.Status204NoContent);
		}
		#endregion Using HybridModelBinding
	}
}
