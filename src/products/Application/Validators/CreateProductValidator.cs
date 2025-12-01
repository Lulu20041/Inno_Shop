using FluentValidation;
using System;
using Application.Commands;
    
namespace Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name of product is required")
                .MaximumLength(200).WithMessage("Name shouldn't be longer than 200 symbols");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description shouldn't be longer than 1000 symbols");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price should be more than zero");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User should be specified");
        }
    }
}
