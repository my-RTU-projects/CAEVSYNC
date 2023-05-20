using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CAEVSYNC.Common.Models;

[JsonConverter(typeof(StringEnumConverter))]  
public enum MicrosoftRecurrencePatternType
{
    [EnumMember(Value = "daily")]
    DAILY,
    
    [EnumMember(Value = "weekly")]
    WEEKLY,
    
    [EnumMember(Value = "absoluteMonthly")]
    ABSOLUTE_MONTHLY,
    
    [EnumMember(Value = "relativeMonthly")]
    RELATIVE_MONTHLY,
    
    [EnumMember(Value = "absoluteYearly")]
    ABSOLUTE_YEARLY,
    
    [EnumMember(Value = "relativeYearly")]
    RELATIVE_YEARLY
}