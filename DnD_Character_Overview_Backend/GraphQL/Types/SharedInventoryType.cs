namespace GraphQL;

public class SharedInventoryType : ObjectType<SharedInventory>
{
    protected override void Configure(IObjectTypeDescriptor<SharedInventory> descriptor)
    {
        descriptor.Field(si => si.Id).Type<NonNullType<IdType>>();
        descriptor.Field(si => si.InventoryItems).Type<ListType<InventoryItemType>>();
    }
}