using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Utils
{
    public class JsonHelper
    {
        public static string SerializeWithUtf8(Object obj)
        { 
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All) });
        }

        public static TValue? Deserialize<TValue>(string jsonStr)
        {
            return JsonSerializer.Deserialize<TValue>(jsonStr);
        }
    }
}