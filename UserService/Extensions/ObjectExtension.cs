using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(object source)
        {
            return JsonConvert.SerializeObject(source);
        }
        public static string ToJsonIndented(object source)
        {

            return JsonConvert.SerializeObject(source, Newtonsoft.Json.Formatting.Indented);
        }
        public static T FromJson<T>(object source)
        {
            return JsonConvert.DeserializeObject<T>(source as string);
        }
    }
}
