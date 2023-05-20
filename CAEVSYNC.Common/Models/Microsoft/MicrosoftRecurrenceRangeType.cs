using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CAEVSYNC.Common.Models;

[JsonConverter(typeof(StringEnumConverter))]  
public enum MicrosoftRecurrenceRangeType
{
    [EnumMember(Value = "numbered")]
    NUMBERED,
    
    [EnumMember(Value = "endDate")]
    END_DATE,
    
    [EnumMember(Value = "noEnd")]
    NO_END,
}