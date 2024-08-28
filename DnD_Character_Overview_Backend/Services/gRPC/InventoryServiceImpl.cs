using Grpc.Core;
using System.Threading.Tasks;
using Protos;
using System.Linq;

public class InventoryServiceImpl : Protos.InventoryService.InventoryServiceBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryServiceImpl(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    public override async Task<InventoryItemsResponse> GetInventoryItems(GetInventoryItemsRequest request, ServerCallContext context)
    {
        var items = await _inventoryService.GetInventoryItemsForCharacterAsync(request.CharacterId, request.Type);
        var response = new InventoryItemsResponse();

        response.Items.AddRange(items.Select(item => new Protos.InventoryItem
        {
            Id = item.Id,
            PlayerCharacterId = item.PlayerCharacterId ?? 0,
            DmCharacterId = item.DMCharacterId ?? 0,
            SharedInventoryId = item.SharedInventoryId ?? 0,
            Name = item.ItemName ?? string.Empty,
            Quantity = item.Quantity,
            Weight = (float)item.Weight,
            WeightUnit = item.WeightUnit ?? "lb"
        }));

        return response;
    }

    public override async Task<InventoryItemResponse> GetInventoryItemById(GetInventoryItemByIdRequest request, ServerCallContext context)
    {
        var item = await _inventoryService.GetInventoryItemByIdAsync(request.CharacterId, request.ItemId, request.Type);
        if (item == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Inventory item not found."));
        }

        var response = new InventoryItemResponse
        {
            Item = new Protos.InventoryItem
            {
                Id = item.Id,
                PlayerCharacterId = item.PlayerCharacterId ?? 0,
                DmCharacterId = item.DMCharacterId ?? 0,
                SharedInventoryId = item.SharedInventoryId ?? 0,
                Name = item.ItemName ?? string.Empty,
                Quantity = item.Quantity,
                Weight = (float)item.Weight,
                WeightUnit = item.WeightUnit ?? "lb"
            }
        };

        return response;
    }

    public override async Task<AddInventoryItemResponse> AddInventoryItem(AddInventoryItemRequest request, ServerCallContext context)
    {
        var newItem = new Models.InventoryItem
        {
            Id = request.Item.Id,
            PlayerCharacterId = request.Item.PlayerCharacterId == 0 ? null : (int?)request.Item.PlayerCharacterId,
            DMCharacterId = request.Item.DmCharacterId == 0 ? null : (int?)request.Item.DmCharacterId,
            SharedInventoryId = request.Item.SharedInventoryId == 0 ? null : (int?)request.Item.SharedInventoryId,
            ItemName = request.Item.Name,
            Quantity = request.Item.Quantity,
            Weight = (decimal)request.Item.Weight,
            WeightUnit = request.Item.WeightUnit
        };

        await _inventoryService.AddInventoryItemAsync(newItem);
        return new AddInventoryItemResponse { Success = true };
    }

    public override async Task<UpdateInventoryItemResponse> UpdateInventoryItem(UpdateInventoryItemRequest request, ServerCallContext context)
    {
        var updatedItem = new Models.InventoryItem
        {
            Id = request.Item.Id,
            PlayerCharacterId = request.Item.PlayerCharacterId == 0 ? null : (int?)request.Item.PlayerCharacterId,
            DMCharacterId = request.Item.DmCharacterId == 0 ? null : (int?)request.Item.DmCharacterId,
            SharedInventoryId = request.Item.SharedInventoryId == 0 ? null : (int?)request.Item.SharedInventoryId,
            ItemName = request.Item.Name,
            Quantity = request.Item.Quantity,
            Weight = (decimal)request.Item.Weight,
            WeightUnit = request.Item.WeightUnit
        };

        await _inventoryService.UpdateInventoryItemAsync(updatedItem);
        return new UpdateInventoryItemResponse { Success = true };
    }

    public override async Task<DeleteInventoryItemResponse> DeleteInventoryItem(DeleteInventoryItemRequest request, ServerCallContext context)
    {
        bool success = await _inventoryService.DeleteInventoryItemAsync(request.CharacterId, request.ItemId, request.Type);
        
        return new DeleteInventoryItemResponse { Success = success };
    }
}
