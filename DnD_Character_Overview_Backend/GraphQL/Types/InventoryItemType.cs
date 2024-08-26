namespace GraphQL;

public class InventoryItemType : ObjectType<InventoryItem>
{
    protected override void Configure(IObjectTypeDescriptor<InventoryItem> descriptor)
    {
        descriptor.Field(ii => ii.Id).Type<NonNullType<IdType>>();
        descriptor.Field(ii => ii.ItemName).Type<StringType>();
        descriptor.Field(ii => ii.Quantity).Type<IntType>();
        descriptor.Field(ii => ii.Weight).Type<FloatType>();
        descriptor.Field(ii => ii.WeightUnit).Type<StringType>();
    }
}