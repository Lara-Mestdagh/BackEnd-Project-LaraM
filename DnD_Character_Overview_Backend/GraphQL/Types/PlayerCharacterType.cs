namespace GraphQL;

public class PlayerCharacterType : ObjectType<PlayerCharacter>
{
    protected override void Configure(IObjectTypeDescriptor<PlayerCharacter> descriptor)
    {
        descriptor.Field(pc => pc.Id).Type<NonNullType<IdType>>();
        descriptor.Field(pc => pc.Name).Type<StringType>();
        descriptor.Field(pc => pc.Race).Type<StringType>();
        descriptor.Field(pc => pc.PlayerName).Type<StringType>();

        descriptor.Field(pc => pc.Strength).Type<IntType>();
        descriptor.Field(pc => pc.Dexterity).Type<IntType>();
        descriptor.Field(pc => pc.Constitution).Type<IntType>();
        descriptor.Field(pc => pc.Intelligence).Type<IntType>();
        descriptor.Field(pc => pc.Wisdom).Type<IntType>();
        descriptor.Field(pc => pc.Charisma).Type<IntType>();

        descriptor.Field(pc => pc.MaxHP).Type<IntType>();
        descriptor.Field(pc => pc.CurrentHP).Type<IntType>();
        descriptor.Field(pc => pc.TempHP).Type<IntType>();

        descriptor.Field(pc => pc.ArmorClass).Type<IntType>();
        descriptor.Field(pc => pc.WalkingSpeed).Type<IntType>();
        descriptor.Field(pc => pc.FlyingSpeed).Type<IntType>();
        descriptor.Field(pc => pc.SwimmingSpeed).Type<IntType>();


        descriptor.Field(pc => pc.KnownLanguages).Type<ListType<StringType>>();
        descriptor.Field(pc => pc.Resistances).Type<ListType<StringType>>();
        descriptor.Field(pc => pc.Weaknesses).Type<ListType<StringType>>();
        descriptor.Field(pc => pc.Conditions).Type<StringType>();
    }
}