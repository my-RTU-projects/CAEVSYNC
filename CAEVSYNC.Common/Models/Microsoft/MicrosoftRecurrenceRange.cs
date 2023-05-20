using CAEVSYNC.Common.Attributes;
using Newtonsoft.Json;

namespace CAEVSYNC.Common.Models;

public class MicrosoftRecurrenceRange
{
    public MicrosoftRecurrenceRangeType Type { get; set; }
    
    [JsonConverter(typeof(JsonDateCustomConverter))]
    public DateTime StartDate { get; set; }
    
    [JsonConverter(typeof(JsonDateCustomConverter))]
    public DateTime EndDate { get; set; }
    
    public int NumberOfOccurrences { get; set; }
}