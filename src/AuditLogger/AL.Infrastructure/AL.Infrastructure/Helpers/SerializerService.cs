using AL.Infrastructure.Helpers.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace AL.Infrastructure.Helpers
{
    public class SerializerService : ISerializerService
    {
        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
            {
                new StringEnumConverter() { CamelCaseText = true }
            }
            });
        }

        public string Serialize<T>(T obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, new());
        }
    }
}
