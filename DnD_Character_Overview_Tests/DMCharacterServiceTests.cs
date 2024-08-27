using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Repositories; 
using Models; 
using Interfaces;

public class DMCharacterServiceTests
{
    private readonly Mock<IDMCharacterRepository> _mockRepository;
    private readonly IMemoryCache _cache;
    private readonly DMCharacterService _service;

    public DMCharacterServiceTests()
    {
        _mockRepository = new Mock<IDMCharacterRepository>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _service = new DMCharacterService(_mockRepository.Object, _cache);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDMCharacters()
    {
        // Arrange
        var characters = new List<DMCharacter>
        {
            new DMCharacter { Id = 1, Name = "Goblin" },
            new DMCharacter { Id = 2, Name = "Orc" }
        };

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(characters);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCharacter()
    {
        // Arrange
        var character = new DMCharacter { Id = 1, Name = "Goblin" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(character);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Goblin", result.Name);
        _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddAsync_AddsCharacterAndInvalidatesCache()
    {
        // Arrange
        var newCharacter = new DMCharacter { Id = 3, Name = "Dragon" };

        // Act
        await _service.AddAsync(newCharacter);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(newCharacter), Times.Once);
        // Verify that cache was invalidated
        Assert.False(_cache.TryGetValue("AllDMCharacters", out _));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesCharacterAndInvalidatesCache()
    {
        // Arrange
        var existingCharacter = new DMCharacter { Id = 1, Name = "Goblin" };

        _mockRepository.Setup(repo => repo.ExistsAsync(existingCharacter.Id)).ReturnsAsync(true);

        // Act
        await _service.UpdateAsync(existingCharacter);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateAsync(existingCharacter), Times.Once);
        _mockRepository.Verify(repo => repo.ExistsAsync(existingCharacter.Id), Times.Once);
        // Verify that caches were invalidated
        Assert.False(_cache.TryGetValue($"DMCharacter_{existingCharacter.Id}", out _));
        Assert.False(_cache.TryGetValue("AllDMCharacters", out _));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionIfCharacterDoesNotExist()
    {
        // Arrange
        var nonExistingCharacter = new DMCharacter { Id = 99, Name = "NonExistent" };

        _mockRepository.Setup(repo => repo.ExistsAsync(nonExistingCharacter.Id)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(nonExistingCharacter));
        _mockRepository.Verify(repo => repo.ExistsAsync(nonExistingCharacter.Id), Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<DMCharacter>()), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteAsync_DeletesCharacterAndInvalidatesCache()
    {
        // Arrange
        var characterId = 1;
        var character = new DMCharacter { Id = characterId, Name = "Goblin" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(characterId)).ReturnsAsync(character);

        // Act
        await _service.SoftDeleteAsync(characterId);

        // Assert
        _mockRepository.Verify(repo => repo.SoftDeleteAsync(characterId), Times.Once);
        // Verify that caches were invalidated
        Assert.False(_cache.TryGetValue($"DMCharacter_{characterId}", out _));
        Assert.False(_cache.TryGetValue("AllDMCharacters", out _));
    }

    [Fact]
    public async Task SoftDeleteAsync_ThrowsExceptionIfCharacterDoesNotExist()
    {
        // Arrange
        var nonExistingCharacterId = 99;

        _mockRepository.Setup(repo => repo.GetByIdAsync(nonExistingCharacterId)).ReturnsAsync(It.IsAny<DMCharacter>());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.SoftDeleteAsync(nonExistingCharacterId));
        _mockRepository.Verify(repo => repo.SoftDeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrueIfCharacterExists()
    {
        // Arrange
        var characterId = 1;

        _mockRepository.Setup(repo => repo.ExistsAsync(characterId)).ReturnsAsync(true);

        // Act
        var exists = await _service.ExistsAsync(characterId);

        // Assert
        Assert.True(exists);
        _mockRepository.Verify(repo => repo.ExistsAsync(characterId), Times.Once);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalseIfCharacterDoesNotExist()
    {
        // Arrange
        var nonExistingCharacterId = 99;

        _mockRepository.Setup(repo => repo.ExistsAsync(nonExistingCharacterId)).ReturnsAsync(false);

        // Act
        var exists = await _service.ExistsAsync(nonExistingCharacterId);

        // Assert
        Assert.False(exists);
        _mockRepository.Verify(repo => repo.ExistsAsync(nonExistingCharacterId), Times.Once);
    }
}
