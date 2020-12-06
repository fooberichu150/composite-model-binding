using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompositeModelBinding.Binders
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
	public class FromRouteAndQueryAttribute : Attribute, IBindingSourceMetadata
	{
		public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
			new[] { BindingSource.Path, BindingSource.Query }, nameof(FromRouteAndQueryAttribute));
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
	public class UseMultiSourceBinderAttribute : Attribute
	{

	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
	public class FromMultiSourceAttribute : Attribute, IBindingSourceMetadata
	{
		public static readonly string[] PathAndQuery = { "Path", "Query" };

		private Lazy<BindingSource> _bindingSourceLazy;

		public FromMultiSourceAttribute(string displayName)
		{
			DisplayName = displayName;
			_bindingSourceLazy = new Lazy<BindingSource>(() => CompositeBindingSource.Create(GetBindingSources(), DisplayName));
		}

		public string DisplayName { get; set; }
		public string[] BindingSources { get; set; }

		public BindingSource BindingSource => _bindingSourceLazy.Value;

		private IEnumerable<BindingSource> GetBindingSources()
		{
			var bsType = typeof(BindingSource);

			return BindingSources.Select(source =>
			{
				return bsType.GetField(source).GetValue(null) as BindingSource;
			});
		}
	}
}