using CAEVSYNC.Common.Models;

namespace CAEVSYNC.ConnectedAccounts.Clients;

public interface ICalendarClient
{
    Task<List<CalendarModel>> GetCalendarsAsync(string userId, string accountId);

    Task<EventModel> GetEventAsync(string userId, string accountId, string calendarId, string eventId);
    
    Task<List<EventModel>> GetEventsAsync(string userId, string accountId, string calendarId);

    Task<string> CreateEventAsync(string userId, string accountId, string calendarId, EventModel eventModel);

    Task UpdateEventAsync(string userId, string accountId, string calendarId, string eventId, EventModel eventModel);

    Task DeleteEventAsync(string userId, string accountId, string calendarId, string eventId);

    void ResetEventPageIterator();

    bool HasNextEventPage();
}