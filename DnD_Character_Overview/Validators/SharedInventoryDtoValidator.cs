namespace Validators;

public class SharedInventoryDtoValidator : AbstractValidator<SharedInventoryDTO>
{
    public SharedInventoryDtoValidator()
    {
        RuleForEach(inventory => inventory.InventoryItems).SetValidator(new InventoryItemDtoValidator());
    }
}
