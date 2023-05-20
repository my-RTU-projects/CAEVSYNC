using CAEVSYNC.Common.Extentions;
using CAEVSYNC.Common.Models;
using CAEVSYNC.ConnectedAccounts.Clients;
using CAEVSYNC.Data;
using CAEVSYNC.Data.Entities;
using CAEVSYNC.Services.EventTransformation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CAEVSYNC.Services;

public class SyncRuleProcessModel
{
    public int Id { get; set; }
    public string OriginCalendarId { get; set; }
    public string TargetCalendarId { get; set; }
    public string OriginCalendarAccountId { get; set; }
    public string TargetCalendarAccountId { get; set; }
    public ICalendarClient OriginCalendarClient { get; set; }
    public ICalendarClient TargetCalendarClient { get; set; }
    public List<EventTransformationStep> EventTransformationSteps { get; set; } = new();
}

public class CalendarSyncService
{
    private readonly CaevsyncDbContext _dbContext;
    private readonly CalendarClientFactory _calendarClientFactory;
    private readonly CalendarService _calendarService;
    private readonly IMemoryCache _memoryCache;
    private readonly EventTransformationServiceFactory _eventTransformationServiceFactory;
    
    public CalendarSyncService(
        CaevsyncDbContext dbContext,
        CalendarClientFactory calendarClientFactory, 
        CalendarService calendarService, 
        IMemoryCache memoryCache,
        EventTransformationServiceFactory eventTransformationServiceFactory)
    {
        _dbContext = dbContext;
        _calendarClientFactory = calendarClientFactory;
        _calendarService = calendarService;
        _memoryCache = memoryCache;
        _eventTransformationServiceFactory = eventTransformationServiceFactory;
    }

    public async Task<bool> CheckIsSyncJobActiveAsync(string userId)
    {
        _memoryCache.TryGetValue($"syncJob-{userId}", out bool isJobActive);
        return isJobActive;
    }

    public async Task StartSynchronizationAsync(string userId)
    {
        _memoryCache.Set($"syncJob-{userId}", true);

        try
        {
            var userAccountIds = await _dbContext.ConnectedAccounts
                .AsNoTracking()
                .Where(a => a.UserToAccountConnections.Any(c => c.UserId == userId))
                .Select(a => a.Id)
                .ToListAsync();

            foreach (var accountId in userAccountIds)
            {
                await UpdateCalendarListAsync(userId, accountId);
            }

            var syncRules = await PrepareSyncRulesAsync(userId);

            foreach (var syncRule in syncRules)
            {
                try
                {
                    await ApplySyncRuleAsync(userId, syncRule);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            _memoryCache.Set($"syncJob-{userId}", false);
        }
    }

    private async Task UpdateCalendarListAsync(string userId, string accountId)
    {
        var account = await _dbContext.ConnectedAccounts.FindAsync(accountId);

        if (account == null)
            throw new ArgumentException($"Account wit id = {accountId} does not exist");
        
        var calendarClient = _calendarClientFactory.CreateCalendarClient(account.AccountType);
        
        var calendars = await calendarClient.GetCalendarsAsync(userId, accountId);

        foreach (var calendar in calendars)
        {
            var existingCalendar = await _dbContext.Calendars
                .Include(c => c.AccountToCalendarConnections)
                .FirstOrDefaultAsync(c => c.Id == calendar.CalendarIdByProvider);
            
            if (existingCalendar == null)
            {
                existingCalendar = new Calendar()
                {
                    Id = calendar.CalendarIdByProvider,
                    AccountToCalendarConnections = new List<AccountToCalendarConnection>()
                };

                _dbContext.Add(existingCalendar);
            }

            var existingConnection = existingCalendar.AccountToCalendarConnections
                .FirstOrDefault(acc => acc.AccountId == accountId);
            
            if (existingConnection == null) {
                existingCalendar.AccountToCalendarConnections.Add(new AccountToCalendarConnection
                {
                    ReadOnly = calendar.ReadOnly,
                    AccountId = account.Id
                });
            }
            else
            {
                existingConnection.ReadOnly = calendar.ReadOnly;
            }
            
            existingCalendar.Title = calendar.Title;
            existingCalendar.ColorHex = calendar.ColorHex;
        }

        var updatedCalendarIds = calendars.Select(c => c.CalendarIdByProvider);
        var calendarsToDelete = await _dbContext.Calendars
            .Where(c => c.AccountToCalendarConnections.Any(acc => acc.AccountId == accountId))
            .Where(c => updatedCalendarIds.All(id => id != c.Id))
            .ToListAsync();

        foreach (var calendar in calendarsToDelete)
        {
            await _calendarService.DeleteCalendarAsync(accountId, calendar.Id);
        }
        
        await _dbContext.SaveChangesAsync();
    }

    private async Task<List<SyncRuleProcessModel>> PrepareSyncRulesAsync(string userId)
    {
        var syncRules = await _dbContext.SyncRules
            .AsNoTracking()
            // Include event time range expand data
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationTimeExpandStepData)
            // Include replace step data
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationIntReplaceStepData)
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationStringReplaceStepData)
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationBoolReplaceStepData)
            // Include filter step data
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationIntFilterData)
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationStringFilterData)
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationBoolFilterData)
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationDateTimeFilterData)
            // Include event time range expand data
            .Include(sr => sr.EventTransformationSteps)
            .ThenInclude(ets => ets.EventTransformationTimeExpandStepData)
            // Include calendars
            .Include(sr => sr.OriginCalendar)
            .ThenInclude(c => c.AccountToCalendarConnections)
            .ThenInclude(acc => acc.Account)
            .Include(sr => sr.TargetCalendar)
            .ThenInclude(c => c.AccountToCalendarConnections)
            .ThenInclude(acc => acc.Account)
            .Where(sr => sr.UserId == userId)
            .Select(sr => new
            {
                SyncRule = sr,
                OriginCalendarAccount = sr.OriginCalendar.AccountToCalendarConnections.First().Account,
                TargetCalendarAccount = sr.TargetCalendar.AccountToCalendarConnections.First().Account,
            })
            .Select(o => new SyncRuleProcessModel
            {
                Id = o.SyncRule.Id,
                EventTransformationSteps = o.SyncRule.EventTransformationSteps,
                OriginCalendarId = o.SyncRule.OriginCalendarId,
                TargetCalendarId = o.SyncRule.TargetCalendarId,
                OriginCalendarAccountId = o.OriginCalendarAccount.Id,
                TargetCalendarAccountId = o.TargetCalendarAccount.Id,
                OriginCalendarClient = _calendarClientFactory.CreateCalendarClient(o.OriginCalendarAccount.AccountType),
                TargetCalendarClient = _calendarClientFactory.CreateCalendarClient(o.TargetCalendarAccount.AccountType)
            }).ToListAsync();

        return syncRules;
    }

    private async Task ApplySyncRuleAsync(string userId, SyncRuleProcessModel syncRule)
    {
        var syncedEventIds = new List<string>();

        while (syncRule.OriginCalendarClient.HasNextEventPage())
        {
            var eventsInOriginCalendar = await syncRule.OriginCalendarClient
                .GetEventsAsync(userId, syncRule.OriginCalendarAccountId, syncRule.OriginCalendarId);
            
            //TODO: apply all transformation steps
            var eventsToSync = eventsInOriginCalendar
                .Select(e => TransformEventAsync(e, syncRule).Result)
                .Where(e => e != null)
                .ToList();

            foreach (var eventModel in eventsToSync)
            {
                var success = await TryUpdateEventInTargetCalendarAsync(userId, syncRule, eventModel, syncedEventIds);
                if (!success)
                    await CreateEventInTargetCalendarAsync(userId, syncRule, eventModel, syncedEventIds);
            }
        }
        
        syncRule.OriginCalendarClient.ResetEventPageIterator();
        
        await _dbContext.SaveChangesAsync();

        await CollectDetachedEventsAsync(userId, syncRule, syncedEventIds);
    }

    private async Task<bool> TryUpdateEventInTargetCalendarAsync(
        string userId, 
        SyncRuleProcessModel syncRule, 
        EventModel eventModel,
        List<string> syncedEventIds)
    {
        var syncedEventData = await _dbContext.SyncedEventData
            .Where(d => d.EventIdInOriginCalendar == eventModel.EventIdByProvider)
            .Where(d => d.SyncRule.TargetCalendarId == syncRule.TargetCalendarId)
            .FirstOrDefaultAsync();

        if (syncedEventData == null)
            return false;
        
        var eventInTarget = await syncRule.TargetCalendarClient.GetEventAsync(
            userId,
            syncRule.TargetCalendarAccountId,
            syncRule.TargetCalendarId,
            syncedEventData.EventIdInTargetCalendarId);

        if (eventInTarget != null) // update
        {
            // TODO: Check diff
            await syncRule.TargetCalendarClient.UpdateEventAsync(
                userId, 
                syncRule.TargetCalendarAccountId,
                syncRule.TargetCalendarId,
                syncedEventData.EventIdInTargetCalendarId,
                eventModel);

            syncedEventIds.Add(syncedEventData.EventIdInTargetCalendarId);

            return true;
        }
                    
        _dbContext.Remove(syncedEventData);
        await _dbContext.SaveChangesAsync();

        return false;
    }

    private async Task CreateEventInTargetCalendarAsync(
        string userId,
        SyncRuleProcessModel syncRule,
        EventModel eventModel,
        List<string> syncedEventIds)
    {
        var idInTargetCalendar = await syncRule.TargetCalendarClient.CreateEventAsync(
            userId, 
            syncRule.TargetCalendarAccountId,
            syncRule.TargetCalendarId, 
            eventModel);

        var syncedEventData = new SyncedEventData 
        { 
            SyncRuleId = syncRule.Id, 
            EventIdInOriginCalendar = eventModel.EventIdByProvider, 
            EventIdInTargetCalendarId = idInTargetCalendar
        };

        _dbContext.Add(syncedEventData);
        await _dbContext.SaveChangesAsync();

        syncedEventIds.Add(syncedEventData.EventIdInTargetCalendarId);
    }

    private async Task CollectDetachedEventsAsync(    
        string userId, 
        SyncRuleProcessModel syncRule,
        List<string> syncedEventIds)
    {
        var syncedEventDataToDelete = await _dbContext.SyncedEventData
            .Where(d => d.SyncRule.OriginCalendarId == syncRule.OriginCalendarId)
            .Where(d => d.SyncRule.TargetCalendarId == syncRule.TargetCalendarId)
            .Where(d => syncedEventIds.All(id => id != d.EventIdInTargetCalendarId))
            .ToListAsync();

        foreach (var syncedEventData in syncedEventDataToDelete)
        {
            syncRule.TargetCalendarClient.DeleteEventAsync(
                userId, 
                syncRule.TargetCalendarAccountId,
                syncRule.TargetCalendarId, 
                syncedEventData.EventIdInTargetCalendarId);
        }

        _dbContext.SyncedEventData.RemoveRange(syncedEventDataToDelete);

        await _dbContext.SaveChangesAsync();
    }
    
    private async Task<EventModel> TransformEventAsync(EventModel eventModel, SyncRuleProcessModel syncRule)
    {
        var transformedEventModel = eventModel.Clone();

        foreach (var step in syncRule.EventTransformationSteps)
        {
            var transformationService = _eventTransformationServiceFactory
                .CreateEventTransformationService(step.TransformationType);

            transformedEventModel = await transformationService.TransformEventAsync(transformedEventModel, step);
        }

        return transformedEventModel;
    }
}