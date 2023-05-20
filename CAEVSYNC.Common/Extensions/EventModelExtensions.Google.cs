using CAEVSYNC.Common.Models;
using CAEVSYNC.Common.Models.Google;

namespace CAEVSYNC.Common.Extentions;

public static partial class EventModelExtensions
{
    public static EventModel ToEventModel(this GoogleEventModel googleEventModel, string calendarId, string eventId)
    {
        var eventModel = new EventModel
        {
            EventIdByProvider = eventId,
            CalendarIdByProvider = calendarId,
            Title = googleEventModel.Summary,
            Description = googleEventModel.Description,
            FromDateTime = googleEventModel.Start.DateTime?.ToUniversalTime() ?? googleEventModel.Start.Date?.ToUniversalTime(),
            ToDateTime = googleEventModel.End.DateTime?.ToUniversalTime() ?? googleEventModel.End.Date?.ToUniversalTime(),
            FromDateTimeZone = googleEventModel.Start.TimeZone,
            ToDateTimeZone = googleEventModel.End.TimeZone,
            Location = googleEventModel.Location,
            IsReminderOn = false, // TODO
            ReminderMinutesBeforeStart = 0,
            IsAllDay = googleEventModel.Start.DateTime == null,
            RecurrencePattern = googleEventModel.Recurrence?.FirstOrDefault()
        };

        return eventModel;
    }
    
    public static GoogleEventModel ToGoogleEventModel(this EventModel eventModel)
    {
        return new GoogleEventModel()
        {
            Summary = eventModel.Title,
            Description = eventModel.Description,
            Start = new GoogleEventDateTimeModel()
            {
                Date = eventModel.IsAllDay ? eventModel.FromDateTime : null,
                DateTime = !eventModel.IsAllDay ? eventModel.FromDateTime : null,
                TimeZone = eventModel.FromDateTimeZone ?? "UTC"
            },
            End = new GoogleEventDateTimeModel()
            {
                Date = eventModel.IsAllDay ? eventModel.ToDateTime : null,
                DateTime = !eventModel.IsAllDay ? eventModel.ToDateTime : null,
                TimeZone = eventModel.ToDateTimeZone ?? "UTC"
            },
            Location = eventModel.Location,
            Recurrence = new [] { eventModel.RecurrencePattern }
        };
    }
}