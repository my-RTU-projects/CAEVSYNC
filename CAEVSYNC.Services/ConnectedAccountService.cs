using CAEVSYNC.Common.Models;
using CAEVSYNC.Data;
using Microsoft.EntityFrameworkCore;

namespace CAEVSYNC.Services;

public class ConnectedAccountService
{
    private readonly CaevsyncDbContext _dbContext;
    private readonly CalendarService _calendarService;
    
    public ConnectedAccountService(CaevsyncDbContext dbContext, CalendarService calendarService)
    {
        _dbContext = dbContext;
        _calendarService = calendarService;
    }

    public async Task<List<ConnectedAccountModel>> GetConnectedAccountsAsync(string userId)
    {
        var accounts = await _dbContext.ConnectedAccounts
            .AsNoTracking()
            .Where(a => a.UserToAccountConnections.Any(uac => uac.UserId == userId))
            .Select(a => new ConnectedAccountModel
            {
                Id = a.Id,
                Title = a.Title,
                AccountType = a.AccountType,
                AccountStatus = a.AccountStatus
            })
            .ToListAsync();

        return accounts;
    }

    public async Task DeleteAccountAsync(string userId, string accountId)
    {
        var account = await _dbContext.ConnectedAccounts
            .Include(a => a.UserToAccountConnections)
            .FirstOrDefaultAsync(a => a.Id == accountId);
        
        await _calendarService.DeleteCalendarsAsync(accountId);

        if (account.UserToAccountConnections.Count > 1)
        {
            var connection = account.UserToAccountConnections
                .FirstOrDefault(c => c.UserId == userId);
            _dbContext.UserToAccountConnections.Remove(connection);
        }
        else
        {
            _dbContext.ConnectedAccounts.Remove(account);
        }
        
        await _dbContext.SaveChangesAsync();
    }
}