using Newtonsoft.Json;

namespace CAEVSYNC.Common.Models.Google;

public class GoogleEventRemindersModel
{
    [JsonProperty("overrides")]
    public GoogleEventReminderModel[] Overrides { get; set; }
}