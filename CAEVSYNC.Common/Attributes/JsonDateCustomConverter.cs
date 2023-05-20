using Newtonsoft.Json.Converters;

namespace CAEVSYNC.Common.Attributes;

public class JsonDateCustomConverter : IsoDateTimeConverter
{
    public JsonDateCustomConverter()
    {
        DateTimeFormat = "yyyy-MM-dd";
    }

    public JsonDateCustomConverter(string format)
    {
        DateTimeFormat = format;
    }
}