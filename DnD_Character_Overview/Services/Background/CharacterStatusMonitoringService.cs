using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services;

public class CharacterStatusMonitoringService : BackgroundService
{
    private readonly ILogger<CharacterStatusMonitoringService> _logger;
    private readonly IServiceScopeFactory _scopeFactory; // Use IServiceScopeFactory for scoped services
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every minute

    public CharacterStatusMonitoringService(ILogger<CharacterStatusMonitoringService> logger,
                                            IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Character Status Monitoring Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var playerCharacterService = scope.ServiceProvider.GetRequiredService<IPlayerCharacterService>();
                var dmCharacterService = scope.ServiceProvider.GetRequiredService<IDMCharacterService>();

                try
                {
                    // Fetch and monitor player characters
                    var playerCharacters = await playerCharacterService.GetAllAsync();
                    foreach (var character in playerCharacters)
                    {
                        if (character.CurrentHP < 10) // Example condition
                        {
                            _logger.LogWarning($"Character {character.Name} is low on HP.");
                            // Trigger notification or other actions
                        }
                    }

                    // Fetch and monitor DM characters
                    var dmCharacters = await dmCharacterService.GetAllAsync();
                    foreach (var character in dmCharacters)
                    {
                        if (character.IsAlive == false) // Example condition
                        {
                            _logger.LogWarning($"Character {character.Name} is no longer alive.");
                            // Trigger notification or other actions
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while monitoring character status.");
                }
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Character Status Monitoring Service stopped.");
    }
}