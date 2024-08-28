public class DMCharacterValidator : AbstractValidator<DMCharacter>
{
    public DMCharacterValidator()
    {
        RuleFor(dc => dc.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters.");

        RuleFor(dc => dc.Race)
            .NotEmpty().WithMessage("Race is required.")
            .Length(1, 255).WithMessage("Race must be between 1 and 255 characters.");

        RuleFor(dc => dc.Strength)
            .InclusiveBetween(1, 20).WithMessage("Strength must be between 1 and 20.");

        RuleFor(dc => dc.Dexterity)
            .InclusiveBetween(1, 20).WithMessage("Dexterity must be between 1 and 20.");

        RuleFor(dc => dc.Constitution)
            .InclusiveBetween(1, 20).WithMessage("Constitution must be between 1 and 20.");

        RuleFor(dc => dc.Intelligence)
            .InclusiveBetween(1, 20).WithMessage("Intelligence must be between 1 and 20.");

        RuleFor(dc => dc.Wisdom)
            .InclusiveBetween(1, 20).WithMessage("Wisdom must be between 1 and 20.");

        RuleFor(dc => dc.Charisma)
            .InclusiveBetween(1, 20).WithMessage("Charisma must be between 1 and 20.");

        RuleFor(dc => dc.MaxHP)
            .GreaterThan(0).WithMessage("Max HP must be greater than 0.");

        RuleFor(dc => dc.CurrentHP)
            .GreaterThanOrEqualTo(0).WithMessage("Current HP cannot be negative.")
            .LessThanOrEqualTo(dc => dc.MaxHP).WithMessage("Current HP cannot exceed Max HP.");

        RuleFor(dc => dc.ArmorClass)
            .GreaterThan(0).WithMessage("Armor Class must be greater than 0.");

        RuleFor(dc => dc.WalkingSpeed)
            .GreaterThan(0).WithMessage("Walking Speed must be greater than 0.");
        
        RuleFor(pc => pc.KnownLanguages)
            .Must(languages => languages != null && languages.Count > 0).WithMessage("Known Languages cannot be empty.");
        
        RuleFor(pc => pc)
            .Must(pc => pc.IsAlive || pc.CurrentHP == 0)
            .WithMessage("A dead character must have 0 current HP.");

        RuleFor(dc => dc.Description)
        .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(dc => dc.LegendaryActions)
            .MaximumLength(500).WithMessage("Legendary Actions cannot exceed 500 characters.");

        RuleFor(dc => dc.SpecialAbilities)
            .MaximumLength(500).WithMessage("Special Abilities cannot exceed 500 characters.");
    }
}