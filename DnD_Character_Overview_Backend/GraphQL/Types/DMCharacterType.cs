namespace GraphQL;

public class DMCharacterType : ObjectType<DMCharacter>
{
    protected override void Configure(IObjectTypeDescriptor<DMCharacter> descriptor)
    {
        descriptor.Field(dm => dm.Id).Type<NonNullType<IdType>>();
        descriptor.Field(dm => dm.Name).Type<StringType>();
        descriptor.Field(dm => dm.Race).Type<StringType>();

        descriptor.Field(dm => dm.Strength).Type<IntType>();
        descriptor.Field(dm => dm.Dexterity).Type<IntType>();
        descriptor.Field(dm => dm.Constitution).Type<IntType>();
        descriptor.Field(dm => dm.Intelligence).Type<IntType>();
        descriptor.Field(dm => dm.Wisdom).Type<IntType>();
        descriptor.Field(dm => dm.Charisma).Type<IntType>();
        
        descriptor.Field(dm => dm.MaxHP).Type<IntType>();
        descriptor.Field(dm => dm.CurrentHP).Type<IntType>();
        descriptor.Field(dm => dm.TempHP).Type<IntType>();
        
        descriptor.Field(dm => dm.ArmorClass).Type<IntType>();
        descriptor.Field(dm => dm.WalkingSpeed).Type<IntType>();
        descriptor.Field(dm => dm.FlyingSpeed).Type<IntType>();
        descriptor.Field(dm => dm.SwimmingSpeed).Type<IntType>();
        
        descriptor.Field(dm => dm.KnownLanguages).Type<ListType<StringType>>();
        descriptor.Field(dm => dm.Resistances).Type<ListType<StringType>>();
        descriptor.Field(dm => dm.Weaknesses).Type<ListType<StringType>>();
        descriptor.Field(dm => dm.Conditions).Type<StringType>();
        
        descriptor.Field(dm => dm.LegendaryActions).Type<StringType>();
        descriptor.Field(dm => dm.SpecialAbilities).Type<StringType>();
    }
}