using CAEVSYNC.Services;
using Microsoft.AspNetCore.Identity;

namespace CAEVSYNC.SyncWorker;

public class CalendarSyncWorker : BackgroundService
{
    private readonly ILogger<CalendarSyncWorker> _logger;
    private readonly IServiceScopeFactory _factory;

    public CalendarSyncWorker(ILogger<CalendarSyncWorker> logger, IServiceScopeFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
            
            var userManager = asyncScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var users = userManager.Users.ToList();
            
            CalendarSyncService calendarSyncService = asyncScope.ServiceProvider.GetRequiredService<CalendarSyncService>();

            foreach (var user in users)
            {
                await calendarSyncService.StartSynchronizationAsync(user.Id);
            }
            
            await Task.Delay(600000, stoppingToken);
        }
    }
}