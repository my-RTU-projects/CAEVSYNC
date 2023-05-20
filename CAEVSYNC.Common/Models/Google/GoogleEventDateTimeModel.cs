using CAEVSYNC.Common.Attributes;
using Newtonsoft.Json;

namespace CAEVSYNC.Common.Models.Google;

public class GoogleEventDateTimeModel
{
    [JsonProperty("date")]
    [JsonConverter(typeof(JsonDateCustomConverter))]
    public DateTime? Date { get; set; }
    
    [JsonProperty("dateTime")]
    public DateTime? DateTime { get; set; }
    
    [JsonProperty("timeZone")]
    public string? TimeZone { get; set; }
}