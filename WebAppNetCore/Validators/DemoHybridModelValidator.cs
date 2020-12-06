using System;
using FluentValidation;
using WebAppNetCore.Models;

namespace WebAppNetCore.Validators
{
	public class DemoHybridModelValidator : AbstractValidator<DemoHybridModel>
	{
		public DemoHybridModelValidator()
		{
			RuleFor(x => x.Id)
				.NotNull().WithMessage("Id must have a value")
				.InclusiveBetween(1, 255).WithMessage("Id is dumb and has to be between 1 and 255");

			RuleFor(x => x.Blah)
				.Must(x => string.Compare(x, "Bloop", StringComparison.OrdinalIgnoreCase) == 0)
				.WithMessage("Blah must be \"Bloop\".");
		}
	}

	public class ParentModelValidator : AbstractValidator<ParentModel>
	{
		public ParentModelValidator()
		{
			RuleFor(x => x.Id)
				.GreaterThanOrEqualTo(1).WithMessage("Id must be greater than or equal to 1");

			//RuleFor(x => x.Nested)
			//	.SetValidator(new NestedModelValidator());
		}
	}

	public class NestedModelValidator : AbstractValidator<NestedModel>
	{
		public NestedModelValidator()
		{
			RuleFor(x => x.Value1)
				.NotEmpty().WithMessage("Value1 cannot be empty");
		}
	}
}
