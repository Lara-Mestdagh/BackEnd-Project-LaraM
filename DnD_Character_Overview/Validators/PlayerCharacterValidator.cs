namespace Validators;

public class PlayerCharacterValidator : AbstractValidator<PlayerCharacter>
{
    public PlayerCharacterValidator()
    {
        RuleFor(pc => pc.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters.");

        RuleFor(pc => pc.Race)
            .NotEmpty().WithMessage("Race is required.")
            .Length(1, 255).WithMessage("Race must be between 1 and 255 characters.");

        RuleFor(pc => pc.Strength)
            .InclusiveBetween(1, 20).WithMessage("Strength must be between 1 and 20.");

        RuleFor(pc => pc.Dexterity)
            .InclusiveBetween(1, 20).WithMessage("Dexterity must be between 1 and 20.");

        RuleFor(pc => pc.Constitution)
            .InclusiveBetween(1, 20).WithMessage("Constitution must be between 1 and 20.");

        RuleFor(pc => pc.Intelligence)
            .InclusiveBetween(1, 20).WithMessage("Intelligence must be between 1 and 20.");

        RuleFor(pc => pc.Wisdom)
            .InclusiveBetween(1, 20).WithMessage("Wisdom must be between 1 and 20.");

        RuleFor(pc => pc.Charisma)
            .InclusiveBetween(1, 20).WithMessage("Charisma must be between 1 and 20.");

        RuleFor(pc => pc.MaxHP)
            .GreaterThan(0).WithMessage("Max HP must be greater than 0.");

        RuleFor(pc => pc.CurrentHP)
            .GreaterThanOrEqualTo(0).WithMessage("Current HP cannot be negative.")
            .LessThanOrEqualTo(pc => pc.MaxHP).WithMessage("Current HP cannot exceed Max HP.");

        RuleFor(pc => pc.ArmorClass)
            .GreaterThan(0).WithMessage("Armor Class must be greater than 0.");

        RuleFor(pc => pc.WalkingSpeed)
            .GreaterThan(0).WithMessage("Walking Speed must be greater than 0.");
    }
}