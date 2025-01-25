using Newtonsoft.Json;

namespace Application.Common.Extensions;

public static class JsonExtension
{
    public static string Serialize<T>(this T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T? Deserialize<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}