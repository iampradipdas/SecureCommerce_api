using FluentValidation;
using SecureCommerce_api.DTOs.Order;

namespace SecureCommerce_api.Validators.Order
{
    public class CheckoutDtoValidator : AbstractValidator<CheckoutDto>
    {
        public CheckoutDtoValidator()
        {
            RuleFor(x => x.ShippingFullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");

            RuleFor(x => x.ShippingCity)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.ShippingZipCode)
                .NotEmpty().WithMessage("Zip code is required.")
                .MaximumLength(20).WithMessage("Zip code cannot exceed 20 characters.");

            RuleFor(x => x.ShippingCountry)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.");
        }
    }
}
