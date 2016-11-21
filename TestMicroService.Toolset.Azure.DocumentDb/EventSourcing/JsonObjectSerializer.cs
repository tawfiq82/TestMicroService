using DimensionData.Toolset.Serialization;
using Newtonsoft.Json;

namespace TestMicroService.Toolset.Azure.DocumentDb.EventSourcing
{
    /// <summary>
    ///     An implementation of <see cref="IStringSerializer" /> using JSON.
    /// </summary>
    internal class JsonObjectSerializer : IStringSerializer
    {
        /// <summary>
        ///     Serializes the specified item.
        /// </summary>
        /// <param name="item">The item to serialize.</param>
        /// <returns>String representing serialized data.</returns>
        public string Serialize(object item)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new JsonContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            return JsonConvert.SerializeObject(item, serializerSettings);
        }

        /// <summary>
        ///     Deserializes the specified serialized string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedString">The serialized string.</param>
        /// <returns>The deserialized data.</returns>
        public T Deserialize<T>(string serializedString)
            where T : class
        {
            var serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ContractResolver = new JsonContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            return JsonConvert.DeserializeObject<T>(serializedString, serializerSettings);
        }

        /// <summary>
        ///     Creates a new instance of the class..
        /// </summary>
        /// <returns></returns>
        public static JsonObjectSerializer New()
        {
            return new JsonObjectSerializer();
        }
    }
}
