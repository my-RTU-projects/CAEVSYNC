using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CAEVSYNC.Common.Models;

[JsonConverter(typeof(StringEnumConverter))]  
public enum MicrosoftDayOfWeek
{
    [EnumMember(Value = "monday")]
    MONDAY,
    
    [EnumMember(Value = "tuesday")]
    TUESDAY,
    
    [EnumMember(Value = "wednesday")]
    WEDNESDAY,
    
    [EnumMember(Value = "thursday")]
    THURSDAY,
    
    [EnumMember(Value = "friday")]
    FRIDAY,
    
    [EnumMember(Value = "saturday")]
    SATURDAY,
    
    [EnumMember(Value = "sunday")]
    SUNDAY
}