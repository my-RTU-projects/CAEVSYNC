using CAEVSYNC.Common.Models;
using CAEVSYNC.Data;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Services;

public class CalendarService
{
    private readonly CaevsyncDbContext _dbContext;
    private readonly SyncRulesService _syncRulesService;
    
    public CalendarService(CaevsyncDbContext dbContext, SyncRulesService syncRulesService)
    {
        _dbContext = dbContext;
        _syncRulesService = syncRulesService;
    }
    
    public async Task<List<CalendarSelectModel>> GetCalendarsAsync(
        string userId, 
        string? accountId = null)
    {
        var calendars = await _dbContext.Calendars
            .Include(cal => cal.AccountToCalendarConnections)
            .ThenInclude(conn => conn.Account)
            .ThenInclude(account => account.UserToAccountConnections)
            .AsNoTracking()
            .Where(cal => 
                accountId == null ||
                cal.AccountToCalendarConnections.Any(acc => acc.AccountId == accountId))
            .Where(cal =>
                cal.AccountToCalendarConnections.Any(acc =>
                    acc.Account.UserToAccountConnections.Any(uac => uac.UserId == userId)))
            .Select(cal => new CalendarSelectModel
            {
                CalendarIdByProvider = cal.Id,
                Title = cal.Title,
                Account = cal.AccountToCalendarConnections
                    .Where(acc => 
                        acc.Account.UserToAccountConnections.Any(uac => uac.UserId == userId))
                    .Select(conn => conn.Account.Title)
                    .FirstOrDefault() ?? "",
                ReadOnly = cal.AccountToCalendarConnections.All(conn => conn.ReadOnly),
                ColorHex = cal.ColorHex
            })
            .ToListAsync();

        return calendars;
    }

    public async Task<CalendarRenderModel> GetCalendarRenderModelAsync(string userId, string calendarId)
    {
        var calendar = await _dbContext.Calendars
            .AsNoTracking()
            .Include(cal => cal.AccountToCalendarConnections)
            .ThenInclude(conn => conn.Account)
            .Where(cal => cal.Id == calendarId)
            .Select(cal => new
            {
                Calendar = cal,
                Account = cal.AccountToCalendarConnections
                    .Where(acc => 
                        acc.Account.UserToAccountConnections.Any(uac => uac.UserId == userId))
                    .Select(acc => acc.Account)
                    .FirstOrDefault() 
            })
            .Select(o => new CalendarRenderModel
            {
                Id = o.Calendar.Id,
                Title = o.Calendar.Title,
                AccountName = o.Account.Title,
                AccountType = o.Account.AccountType,
                ColorHex = o.Calendar.ColorHex
            })
            .FirstOrDefaultAsync();

        if (calendar == null)
            throw new ArgumentException($"Calendar with id = {calendarId} does not exist");

        return calendar;
    }
    
    public async Task DeleteCalendarsAsync(string accountId)
    {
        var calendars = await _dbContext.Calendars
            .Include(c => c.AccountToCalendarConnections)
            .Where(c => 
                c.AccountToCalendarConnections.Any(acc => acc.AccountId == accountId))
            .ToListAsync();

        foreach (var calendar in calendars)
        {
            await _syncRulesService.DeleteSyncRulesAsync(calendar.Id);
            
            if (calendar.AccountToCalendarConnections.Count > 1)
            {
                var connection = calendar.AccountToCalendarConnections
                    .FirstOrDefault(c => c.AccountId == accountId);
                _dbContext.AccountToCalendarConnections.Remove(connection);
            }
            else
            {
                _dbContext.Calendars.Remove(calendar);
            }
        }

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteCalendarAsync(string accountId, string calendarId)
    {
        var calendar = await _dbContext.Calendars
            .Include(c => c.AccountToCalendarConnections)
            .Where(c => c.Id == calendarId)
            .FirstOrDefaultAsync();

        if (calendar == null)
            throw new ArgumentException($"Calendar with id = {calendarId} does not exist");
        
        await _syncRulesService.DeleteSyncRulesAsync(calendar.Id);
        
        if (calendar.AccountToCalendarConnections.Count > 1)
        {
            var connection = calendar.AccountToCalendarConnections
                .FirstOrDefault(c => c.AccountId == accountId);
            _dbContext.AccountToCalendarConnections.Remove(connection);
        }
        else
        {
            _dbContext.Calendars.Remove(calendar);
        }
                    
        await _dbContext.SaveChangesAsync();
    }
}