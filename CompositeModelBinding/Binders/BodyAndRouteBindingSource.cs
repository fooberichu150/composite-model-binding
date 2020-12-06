using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace CompositeModelBinding.Binders
{
	public class BodyAndRouteBindingSource : BindingSource
	{
		public static readonly BindingSource BodyAndRoute = new BodyAndRouteBindingSource(
			"BodyAndRoute",
			"BodyAndRoute",
			true,
			true
			);

		public BodyAndRouteBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest) : base(id, displayName, isGreedy, isFromRequest)
		{
		}

		public override bool CanAcceptDataFrom(BindingSource bindingSource)
		{
			return bindingSource == Body || bindingSource == this;
		}
	}

	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class FromBodyAndRouteAttribute : Attribute, IBindingSourceMetadata
	{
		public BindingSource BindingSource => BodyAndRouteBindingSource.BodyAndRoute;
	}

	public class BodyAndRouteModelBinder : IModelBinder
	{
		private readonly IModelBinder _bodyBinder;
		private readonly IModelBinder _complexBinder;

		public BodyAndRouteModelBinder(IModelBinder bodyBinder, IModelBinder complexBinder)
		{
			_bodyBinder = bodyBinder;
			_complexBinder = complexBinder;
		}

		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			await _bodyBinder.BindModelAsync(bindingContext);

			if (bindingContext.Result.IsModelSet)
			{
				bindingContext.Model = bindingContext.Result.Model;
			}

			await _complexBinder.BindModelAsync(bindingContext);
		}
	}

	public class BodyAndRouteModelBinderProvider : IModelBinderProvider
	{
		private BodyModelBinderProvider _bodyModelBinderProvider;
		private ComplexObjectModelBinderProvider _complexTypeModelBinderProvider;

		public BodyAndRouteModelBinderProvider(BodyModelBinderProvider bodyModelBinderProvider, ComplexObjectModelBinderProvider complexTypeModelBinderProvider)
		{
			_bodyModelBinderProvider = bodyModelBinderProvider;
			_complexTypeModelBinderProvider = complexTypeModelBinderProvider;
		}

		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			var bodyBinder = _bodyModelBinderProvider.GetBinder(context);
			var complexBinder = _complexTypeModelBinderProvider.GetBinder(context);

			if (context.BindingInfo.BindingSource != null
				&& context.BindingInfo.BindingSource.CanAcceptDataFrom(BodyAndRouteBindingSource.BodyAndRoute))
			{
				return new BodyAndRouteModelBinder(bodyBinder, complexBinder);
			}
			else
			{
				return null;
			}
		}
	}

	public static class BodyAndRouteModelBinderProviderSetup
	{
		public static void InsertBodyAndRouteBinding(this IList<IModelBinderProvider> providers)
		{
			var bodyProvider = providers.Single(provider => provider.GetType() == typeof(BodyModelBinderProvider)) as BodyModelBinderProvider;
			var complexProvider = providers.Single(provider => provider.GetType() == typeof(ComplexObjectModelBinderProvider)) as ComplexObjectModelBinderProvider;

			var bodyAndRouteProvider = new BodyAndRouteModelBinderProvider(bodyProvider, complexProvider);

			providers.Insert(0, bodyAndRouteProvider);
		}
	}
}