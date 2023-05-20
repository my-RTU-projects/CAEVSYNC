using CAEVSYNC.Common.Models.Enums;
using CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;

namespace CAEVSYNC.ConnectedAccounts.Clients;

public class CalendarClientFactory
{
    private readonly GoogleAuthFlowContext _googleAuthFlowContext;
    private readonly MicrosoftAuthFlowContext _microsoftAuthFlowContext;
    
    public CalendarClientFactory(
        GoogleAuthFlowContext googleAuthFlowContext,
        MicrosoftAuthFlowContext microsoftAuthFlowContext)
    {
        _googleAuthFlowContext = googleAuthFlowContext;
        _microsoftAuthFlowContext = microsoftAuthFlowContext;
    }

    public ICalendarClient CreateCalendarClient(AccountType accountType)
    {
        return accountType switch
        {
            AccountType.GOOGLE => new GoogleCalendarClient(_googleAuthFlowContext),
            AccountType.MICROSOFT => new MicrosoftCalendarClient(_microsoftAuthFlowContext),
            _ => throw new ArgumentException("Can't provide service for this account type")
        };
    }
}