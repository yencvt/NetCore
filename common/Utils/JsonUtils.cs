using System;
using System.Text.Json;

namespace common.Utils
{
    public static class JsonUtils
    {
        public static T ConvertJsonToObject<T>(string jsonString)
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static string ConvertObjectToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T ConvertObjectToObject<T>(object obj)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}

