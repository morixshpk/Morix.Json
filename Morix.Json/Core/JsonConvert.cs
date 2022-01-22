using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Morix.Json
{
    public static class JsonConvert
    {
        /// <summary>
        /// Gets or sets a value indicating whether the result string should be formatted for human-readability.
        /// </summary>
        public static bool Beautify { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether JsonObject properties should be written in a deterministic order.
        /// </summary>
        public static bool SortProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include propertis from serialization/deserialization. 
        /// Default true
        /// </summary>
        public static bool IncludeProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include fields from serialization/deserialization. 
        /// Default true
        /// </summary>
        public static bool IncludeFields { get; set; }

        static JsonConvert()
        {
            IncludeFields = true;
            IncludeProperties = true;
        }

        /// <summary>
        /// Serialize any object to string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            var converter = new JsonConverter();
            return converter.Serialize(obj);
        }

        /// <summary>
        /// Deserialize string to given T type
        /// </summary>
        /// <typeparam name="T">Type to be convert to</typeparam>
        /// <param name="json">Json string value to be parsed</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string json)
        {
            try
            {
                var converter = new JsonConverter();
                return converter.Deserialize<T>(json);
            }
            catch
            {

            }
            return default;
        }

        /// <summary>
        /// Parse string into JsonValue objects
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JsonValue Parse(string json)
        {
            return JsonReader.Parse(json);
        }
    }
}