using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace flash_card_api.Libs
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, string> GetPropertyNames(this object obj)
        {
            Dictionary<string, string> kvPairs = new Dictionary<string, string>();

            foreach (var property in obj.GetType().GetProperties())
            {
                // Ignore attributes that are not mapped to the DB
                if (property.GetType() == typeof(NotMappedAttribute))
                    continue;

                var jsonProperty = property.GetCustomAttribute<JsonPropertyAttribute>();
                var jsonPropertyName = jsonProperty?.PropertyName;

                if (jsonPropertyName != null)
                    kvPairs.TryAdd(jsonPropertyName, property.Name);
                else
                    kvPairs.TryAdd(property.Name, property.Name);
            }

            return kvPairs;
        }
    }
}
