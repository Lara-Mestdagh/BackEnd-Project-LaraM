using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;
using Services;

namespace GraphQL;

public class Query
{
    [GraphQLName("getPlayerCharacters")]
    public async Task<IEnumerable<PlayerCharacter>> GetPlayerCharacters([Service] IPlayerCharacterService playerCharacterService)
    {
        return await playerCharacterService.GetAllAsync();
    }

    [GraphQLName("getDMCharacters")]
    public async Task<IEnumerable<DMCharacter>> GetDMCharacters([Service] IDMCharacterService dmCharacterService)
    {
        return await dmCharacterService.GetAllAsync();
    }

    [GraphQLName("getSharedInventory")]
    public async Task<SharedInventory> GetSharedInventory([Service] IInventoryService inventoryService)
    {
        return await inventoryService.GetSharedInventoryAsync();
    }
}