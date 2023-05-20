using Newtonsoft.Json;

namespace CAEVSYNC.Common.Models.Google;

public class GoogleEventModel
{
    [JsonProperty("summary")]
    public string? Summary { get; set; }
    
    [JsonProperty("description")]
    public string? Description { get; set; }
    
    [JsonProperty("start")]
    public GoogleEventDateTimeModel Start { get; set; }
    
    [JsonProperty("end")]
    public GoogleEventDateTimeModel End { get; set; }
    
    [JsonProperty("location")]
    public string? Location { get; set; }
    
    [JsonProperty("recurrence")]
    public string[]? Recurrence { get; set; }
}