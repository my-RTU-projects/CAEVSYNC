using CAEVSYNC.Common.Models;

namespace CAEVSYNC.Common.Extentions;

public static partial class EventModelExtensions
{
    public static EventModel ToEventModel(this MicrosoftEventModel microsoftEventModel, string calendarId, string eventId)
    {
        var eventModel = new EventModel
        {
            EventIdByProvider = eventId,
            CalendarIdByProvider = calendarId,
            Title = microsoftEventModel.Subject,
            Description = microsoftEventModel.Body.Content,
            FromDateTime = microsoftEventModel.Start.DateTime?.ToUniversalTime(),
            ToDateTime = microsoftEventModel.End.DateTime?.ToUniversalTime(),
            FromDateTimeZone = microsoftEventModel.Start.TimeZone,
            ToDateTimeZone = microsoftEventModel.End.TimeZone,
            Location = microsoftEventModel.Location.DisplayName,
            IsReminderOn = microsoftEventModel.IsReminderOn,
            ReminderMinutesBeforeStart = microsoftEventModel.ReminderMinutesBeforeStart,
            IsAllDay = microsoftEventModel.IsAllDay,
            RecurrencePattern = microsoftEventModel.Recurrence.ToICalRecurrencePattern()
        };

        return eventModel;
    }
    
    public static MicrosoftEventModel ToMicrosoftEventModel(this EventModel eventModel)
    {
        var fromDateTime = eventModel.FromDateTime;
        var toDateTime = eventModel.ToDateTime;
        
        if (eventModel.IsAllDay && fromDateTime.HasValue && toDateTime.HasValue)
        {
            var midnight = new DateTime(
                fromDateTime.Value.Year,
                fromDateTime.Value.Month,
                fromDateTime.Value.Day,
                0,
                0,
                0);
            fromDateTime = midnight;
            toDateTime = midnight.AddDays(1);
        }

        var microsoftEventModel = new MicrosoftEventModel
        {
            Subject = eventModel.Title,
            Body = new MicrosoftEventBodyModel
            {
                ContentType = "html",
                Content = eventModel.Description,
            },
            Start = new MicrosoftEventDateTimeModel
            {
                TimeZone = eventModel.FromDateTimeZone ?? "UTC",
                DateTime = fromDateTime
            },
            End = new MicrosoftEventDateTimeModel
            {
                TimeZone = eventModel.ToDateTimeZone ?? "UTC",
                DateTime = toDateTime
            },
            Location = new MicrosoftEventLocationModel
            {
                DisplayName = eventModel.Location
            },
            IsReminderOn = eventModel.IsReminderOn,
            ReminderMinutesBeforeStart = eventModel.ReminderMinutesBeforeStart,
            IsAllDay = eventModel.IsAllDay,
            Recurrence = eventModel.RecurrencePattern.ToMicrosoftRecurrence(eventModel.FromDateTime ?? DateTime.Now)
        };

        return microsoftEventModel;
    }
}