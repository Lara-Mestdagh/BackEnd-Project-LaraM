using FluentValidation;

namespace Validators;

public class CharacterClassDtoValidator : AbstractValidator<CharacterClassDTO>
{
    public CharacterClassDtoValidator()
    {
        RuleFor(cc => cc.PlayerCharacterId)
            .GreaterThan(0).WithMessage("Player Character ID must be greater than 0.");

        RuleFor(cc => cc.ClassName)
            .NotEmpty().WithMessage("Class name is required.")
            .Length(1, 255).WithMessage("Class name must be between 1 and 255 characters.");

        RuleFor(cc => cc.Level)
            .GreaterThan(0).WithMessage("Level must be greater than 0.");
    }
}
