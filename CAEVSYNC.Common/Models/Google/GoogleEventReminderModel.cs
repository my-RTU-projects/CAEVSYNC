using Newtonsoft.Json;

namespace CAEVSYNC.Common.Models.Google;

public class GoogleEventReminderModel
{
    [JsonProperty("minutes")]
    public int Minutes { get; set; }
    
    [JsonProperty("method")]
    public string Method { get; set; }
}