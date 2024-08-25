using FluentValidation;

namespace Validators;

public class PlayerCharacterDtoValidator : AbstractValidator<PlayerCharacterDTO>
{
    public PlayerCharacterDtoValidator()
    {
        RuleFor(pc => pc.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters.");

        RuleFor(pc => pc.Race)
            .MaximumLength(255).WithMessage("Race must be 255 characters or less.");

        RuleFor(pc => pc.PlayerName)
            .NotEmpty().WithMessage("Player name is required.")
            .Length(1, 255).WithMessage("Player name must be between 1 and 255 characters.");

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
    }
}
