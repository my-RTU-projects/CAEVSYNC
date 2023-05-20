using CAEVSYNC.Common.Models;

namespace CAEVSYNC.Common.Extentions;

public static partial class EventModelExtensions
{
    public static EventModel Clone(this EventModel eventModel)
    {
        return new EventModel
        {
            EventIdByProvider = eventModel.EventIdByProvider,
            CalendarIdByProvider = eventModel.CalendarIdByProvider,
            Title = eventModel.Title,
            Description = eventModel.Description,
            FromDateTime = eventModel.FromDateTime,
            ToDateTime = eventModel.ToDateTime,
            FromDateTimeZone = eventModel.FromDateTimeZone,
            ToDateTimeZone = eventModel.ToDateTimeZone,
            Location = eventModel.Location,
            IsReminderOn = eventModel.IsReminderOn,
            ReminderMinutesBeforeStart = eventModel.ReminderMinutesBeforeStart,
            IsAllDay = eventModel.IsAllDay,
            RecurrencePattern = eventModel.RecurrencePattern
        };
    }
}