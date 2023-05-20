using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CAEVSYNC.Common.Models;

[JsonConverter(typeof(StringEnumConverter))]  
public enum MicrosoftWeekIndex
{
    [EnumMember(Value = "first")]
    FIRST,
    
    [EnumMember(Value = "second")]
    SECOND,
    
    [EnumMember(Value = "third")]
    THIRD,
    
    [EnumMember(Value = "fourth")]
    FOURTH,
    
    [EnumMember(Value = "last")]
    LAST
}