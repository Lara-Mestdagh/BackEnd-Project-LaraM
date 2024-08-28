using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Repositories; 
using Models; 
using Interfaces;
using System;
using System.Linq;

public class InventoryServiceTests
{
    private readonly Mock<IInventoryRepository> _mockRepository;
    private readonly InventoryService _service;

    public InventoryServiceTests()
    {
        _mockRepository = new Mock<IInventoryRepository>();
        _service = new InventoryService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetInventoryItemsForCharacterAsync_ReturnsItems()
    {
        // Arrange
        int characterId = 1;
        string type = "player"; // Add type parameter
        var items = new List<InventoryItem>
        {
            new InventoryItem { Id = 1, ItemName = "Sword", Quantity = 1 },
            new InventoryItem { Id = 2, ItemName = "Shield", Quantity = 1 }
        };

        // Setup mock to include type parameter
        _mockRepository.Setup(repo => repo.GetInventoryItemsForCharacterAsync(characterId, type))
                       .ReturnsAsync(items);

        // Act
        var result = await _service.GetInventoryItemsForCharacterAsync(characterId, type);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(repo => repo.GetInventoryItemsForCharacterAsync(characterId, type), Times.Once);
    }

    [Fact]
    public async Task GetInventoryItemByIdAsync_ReturnsItem()
    {
        // Arrange
        int characterId = 1;
        int itemId = 1;
        string type = "player"; // Add type parameter
        var item = new InventoryItem { Id = 1, ItemName = "Sword", Quantity = 1 };

        // Setup mock to include type parameter
        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(characterId, itemId, type))
                       .ReturnsAsync(item);

        // Act
        var result = await _service.GetInventoryItemByIdAsync(characterId, itemId, type);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sword", result.ItemName);
        _mockRepository.Verify(repo => repo.GetInventoryItemByIdAsync(characterId, itemId, type), Times.Once);
    }

    [Fact]
    public async Task AddInventoryItemAsync_AddsItem()
    {
        // Arrange
        var newItem = new InventoryItem { Id = 3, ItemName = "Bow", Quantity = 1 };

        // Act
        await _service.AddInventoryItemAsync(newItem);

        // Assert
        _mockRepository.Verify(repo => repo.AddInventoryItemAsync(newItem), Times.Once);
    }

    [Fact]
    public async Task UpdateInventoryItemAsync_UpdatesItem()
    {
        // Arrange
        var updatedItem = new InventoryItem { Id = 1, ItemName = "Sword", Quantity = 2 };

        // Act
        await _service.UpdateInventoryItemAsync(updatedItem);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateInventoryItemAsync(updatedItem), Times.Once);
    }

    [Fact]
    public async Task DeleteInventoryItemAsync_DeletesItem()
    {
        // Arrange
        string type = "player"; // Add type parameter
        int characterId = 1;
        int itemId = 1;
        var item = new InventoryItem { Id = itemId, ItemName = "Sword", Quantity = 1 };

        // Setup mock to include type parameter
        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(characterId, itemId, type))
                       .ReturnsAsync(item);

        // Act
        var result = await _service.DeleteInventoryItemAsync(characterId, itemId, type);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.DeleteInventoryItemAsync(item), Times.Once);
    }

    [Fact]
    public async Task DeleteInventoryItemAsync_ReturnsFalseWhenItemNotFound()
    {
        // Arrange
        int characterId = 1;
        int itemId = 99;
        string type = "player"; // Add type parameter

        // Setup mock to include type parameter
        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(characterId, itemId, type))
                       .ReturnsAsync((InventoryItem?)null);

        // Act
        var result = await _service.DeleteInventoryItemAsync(characterId, itemId, type);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(repo => repo.DeleteInventoryItemAsync(It.IsAny<InventoryItem>()), Times.Never);
    }

    [Fact]
    public async Task GetSharedInventoryAsync_ReturnsSharedInventory()
    {
        // Arrange
        var sharedInventory = new SharedInventory { Id = 1 };

        _mockRepository.Setup(repo => repo.GetSharedInventoryAsync()).ReturnsAsync(sharedInventory);

        // Act
        var result = await _service.GetSharedInventoryAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockRepository.Verify(repo => repo.GetSharedInventoryAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateSharedInventoryAsync_UpdatesSharedInventory()
    {
        // Arrange
        var sharedInventory = new SharedInventory { Id = 1 };

        // Act
        await _service.UpdateSharedInventoryAsync(sharedInventory);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateSharedInventoryAsync(sharedInventory), Times.Once);
    }

    [Fact]
    public async Task AddItemToSharedInventoryAsync_AddsItemToSharedInventory()
    {
        // Arrange
        int sharedInventoryId = 1;
        var newItem = new InventoryItem { Id = 4, ItemName = "Health Potion", Quantity = 3 };

        // Act
        await _service.AddItemToSharedInventoryAsync(sharedInventoryId, newItem);

        // Assert
        _mockRepository.Verify(repo => repo.AddInventoryItemAsync(newItem), Times.Once);
        Assert.Null(newItem.PlayerCharacterId);
        Assert.Null(newItem.DMCharacterId);
        Assert.Equal(sharedInventoryId, newItem.SharedInventoryId);
    }

    [Fact]
    public async Task MoveItemToSharedInventoryAsync_MovesItemToSharedInventory()
    {
        // Arrange
        int itemId = 1;
        int sharedInventoryId = 1;
        var item = new InventoryItem { Id = itemId, ItemName = "Sword", Quantity = 1, PlayerCharacterId = 1 };

        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(itemId)).ReturnsAsync(item);

        // Act
        await _service.MoveItemToSharedInventoryAsync(itemId, sharedInventoryId);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateInventoryItemAsync(item), Times.Once);
        Assert.Null(item.PlayerCharacterId);
        Assert.Null(item.DMCharacterId);
        Assert.Equal(sharedInventoryId, item.SharedInventoryId);
    }

    [Fact]
    public async Task MoveItemToSharedInventoryAsync_ThrowsExceptionWhenItemNotFound()
    {
        // Arrange
        int itemId = 99;
        int sharedInventoryId = 1;

        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(itemId)).ReturnsAsync((InventoryItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.MoveItemToSharedInventoryAsync(itemId, sharedInventoryId));
        _mockRepository.Verify(repo => repo.UpdateInventoryItemAsync(It.IsAny<InventoryItem>()), Times.Never);
    }

    [Fact]
    public async Task RemoveItemFromInventoryAsync_RemovesItem()
    {
        // Arrange
        int itemId = 1;
        var item = new InventoryItem { Id = itemId, ItemName = "Sword", Quantity = 1 };

        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(itemId)).ReturnsAsync(item);

        // Act
        await _service.RemoveItemFromInventoryAsync(itemId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteInventoryItemAsync(item), Times.Once);
    }

    [Fact]
    public async Task RemoveItemFromInventoryAsync_ThrowsExceptionWhenItemNotFound()
    {
        // Arrange
        int itemId = 99;

        _mockRepository.Setup(repo => repo.GetInventoryItemByIdAsync(itemId)).ReturnsAsync((InventoryItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.RemoveItemFromInventoryAsync(itemId));
        _mockRepository.Verify(repo => repo.DeleteInventoryItemAsync(It.IsAny<InventoryItem>()), Times.Never);
    }
}
