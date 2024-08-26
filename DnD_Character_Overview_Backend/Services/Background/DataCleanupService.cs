using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services;

public class DataCleanupService : BackgroundService
{
    private readonly ILogger<DataCleanupService> _logger;
    private readonly IServiceScopeFactory _scopeFactory; // Use IServiceScopeFactory for scoped services

    public DataCleanupService(ILogger<DataCleanupService> logger,
                                IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Data Cleanup Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var playerCharacterRepository = scope.ServiceProvider.GetRequiredService<IPlayerCharacterRepository>();
                var dmCharacterRepository = scope.ServiceProvider.GetRequiredService<IDMCharacterRepository>();

                try
                {
                    // Check if today is the first day of the month
                    if (DateTime.UtcNow.Day == 1)
                    {
                        // Perform the cleanup operations
                        await playerCharacterRepository.CleanupSoftDeletedAsync();
                        await dmCharacterRepository.CleanupSoftDeletedAsync();

                        _logger.LogInformation("Data cleanup completed successfully.");
                    }
                    else
                    {
                        _logger.LogInformation("It's not the first of the month. Skipping cleanup.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during data cleanup.");
                }
            }

            // Calculate the time until the next check
            var nextCheck = DateTime.UtcNow.AddDays(1).Date; // Start of the next day
            var delay = nextCheck - DateTime.UtcNow;

            await Task.Delay(delay, stoppingToken);
        }

        _logger.LogInformation("Data Cleanup Service stopped.");
    }
}