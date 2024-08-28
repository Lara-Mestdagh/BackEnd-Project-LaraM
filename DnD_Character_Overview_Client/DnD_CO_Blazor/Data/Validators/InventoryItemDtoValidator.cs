public class InventoryItemDtoValidator : AbstractValidator<InventoryItemDTO>
{
    public InventoryItemDtoValidator()
    {
        RuleFor(item => item.ItemName)
            .NotEmpty().WithMessage("Item name is required.")
            .Length(1, 255).WithMessage("Item name must be between 1 and 255 characters.");

        RuleFor(item => item.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(item => item.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Weight cannot be negative.");

        RuleFor(item => item.WeightUnit)
            .NotEmpty().WithMessage("Weight unit is required.")
            .Must(unit => unit == "lb" || unit == "kg").WithMessage("Weight unit must be 'lb' or 'kg'.");
    }
}
