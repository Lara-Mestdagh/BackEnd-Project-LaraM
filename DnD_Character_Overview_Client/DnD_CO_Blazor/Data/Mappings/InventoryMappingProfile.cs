using AutoMapper;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        // Map InventoryItem to InventoryItemDTO and vice versa
        CreateMap<InventoryItem, InventoryItemDTO>().ReverseMap();

        // Map SharedInventory to SharedInventoryDTO and vice versa
        CreateMap<SharedInventory, SharedInventoryDTO>().ReverseMap();
    }
}