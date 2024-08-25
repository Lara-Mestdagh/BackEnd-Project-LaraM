using FluentValidation;

namespace Validators;

public class InventoryItemDtoValidator : AbstractValidator<InventoryItemDTO>
{
    public InventoryItemDtoValidator()
    {
        RuleFor(ii => ii.ItemName)
            .NotEmpty().WithMessage("Item Name is required.")
            .Length(1, 255).WithMessage("Item Name must be between 1 and 255 characters.");

        RuleFor(ii => ii.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(ii => ii.Weight)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");
    }
}
