using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Repositories; 
using Models; 
using Interfaces;

public class PlayerCharacterServiceTests
{
    private readonly Mock<IPlayerCharacterRepository> _mockRepository;
    private readonly Mock<ICharacterClassRepository> _mockClassRepository;
    private readonly IMemoryCache _cache;
    private readonly PlayerCharacterService _service;

    public PlayerCharacterServiceTests()
    {
        _mockRepository = new Mock<IPlayerCharacterRepository>();
        _mockClassRepository = new Mock<ICharacterClassRepository>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _service = new PlayerCharacterService(_mockRepository.Object, _mockClassRepository.Object, _cache);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPlayerCharacters()
    {
        // Arrange
        var characters = new List<PlayerCharacter>
        {
            new PlayerCharacter { Id = 1, Name = "Aragorn" },
            new PlayerCharacter { Id = 2, Name = "Legolas" }
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
    public async Task GetByIdAsync_ReturnsPlayerCharacter()
    {
        // Arrange
        var character = new PlayerCharacter { Id = 1, Name = "Aragorn" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(character);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Aragorn", result.Name);
        _mockRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task AddAsync_AddsPlayerCharacterAndInvalidatesCache()
    {
        // Arrange
        var newCharacter = new PlayerCharacter { Id = 3, Name = "Gimli" };

        // Act
        await _service.AddAsync(newCharacter);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(newCharacter), Times.Once);
        // Verify that cache was invalidated
        Assert.False(_cache.TryGetValue("AllPlayerCharacters", out _));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPlayerCharacterAndInvalidatesCache()
    {
        // Arrange
        var existingCharacter = new PlayerCharacter { Id = 1, Name = "Aragorn" };

        _mockRepository.Setup(repo => repo.ExistsAsync(existingCharacter.Id)).ReturnsAsync(true);

        // Act
        await _service.UpdateAsync(existingCharacter);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateAsync(existingCharacter), Times.Once);
        _mockRepository.Verify(repo => repo.ExistsAsync(existingCharacter.Id), Times.Once);
        // Verify that caches were invalidated
        Assert.False(_cache.TryGetValue($"PlayerCharacter_{existingCharacter.Id}", out _));
        Assert.False(_cache.TryGetValue("AllPlayerCharacters", out _));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsExceptionIfCharacterDoesNotExist()
    {
        // Arrange
        var nonExistingCharacter = new PlayerCharacter { Id = 99, Name = "Unknown" };

        _mockRepository.Setup(repo => repo.ExistsAsync(nonExistingCharacter.Id)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(nonExistingCharacter));
        _mockRepository.Verify(repo => repo.ExistsAsync(nonExistingCharacter.Id), Times.Once);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<PlayerCharacter>()), Times.Never);
    }

    [Fact]
    public async Task SoftDeleteAsync_DeletesPlayerCharacterAndInvalidatesCache()
    {
        // Arrange
        var characterId = 1;
        var character = new PlayerCharacter { Id = characterId, Name = "Aragorn" };

        _mockRepository.Setup(repo => repo.GetByIdAsync(characterId)).ReturnsAsync(character);

        // Act
        await _service.SoftDeleteAsync(characterId);

        // Assert
        _mockRepository.Verify(repo => repo.SoftDeleteAsync(characterId), Times.Once);
        // Verify that caches were invalidated
        Assert.False(_cache.TryGetValue($"PlayerCharacter_{characterId}", out _));
        Assert.False(_cache.TryGetValue("AllPlayerCharacters", out _));
    }

    [Fact]
    public async Task SoftDeleteAsync_ThrowsExceptionIfCharacterDoesNotExist()
    {
        // Arrange
        var nonExistingCharacterId = 99;

        _mockRepository.Setup(repo => repo.GetByIdAsync(nonExistingCharacterId)).ReturnsAsync(It.IsAny<PlayerCharacter>());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.SoftDeleteAsync(nonExistingCharacterId));
        _mockRepository.Verify(repo => repo.SoftDeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrueIfPlayerCharacterExists()
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
    public async Task ExistsAsync_ReturnsFalseIfPlayerCharacterDoesNotExist()
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
