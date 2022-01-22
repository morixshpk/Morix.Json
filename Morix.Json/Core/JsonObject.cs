using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonObject : JsonValue, IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable<JsonValue>
    {
        private readonly IDictionary<string, JsonValue> properties;

        public override int Count
        {
            get
            {
                return properties.Count;
            }
        }

        public override JsonValue this[int index]
        {
            get
            {
                throw new InvalidOperationException("This value does not represent a JArray.");
            }
            set
            {
                throw new InvalidOperationException("This value does not represent a JArray.");
            }
        }

        public override JsonValue this[string key]
        {
            get
            {
                if (this.properties.TryGetValue(key, out var value))
                {
                    return value;
                }
                else
                {
                    return JsonValue.Null;
                }
            }
            set
            {
                this.properties[key] = value;
            }
        }

        public JsonObject() :base(JsonType.Object)
        {
            properties = new Dictionary<string, JsonValue>();
        }

        public JsonObject Add(string key)
        {
            return Add(key, JsonValue.Null);
        }

        public JsonObject Add(string key, JsonValue value)
        {
            this.properties.Add(key, value);
            return this;
        }

        public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
        {
            return properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
        {
            return properties.Values.GetEnumerator();
        }

        public override bool ContainsKey(string key)
        {
            return this.properties.ContainsKey(key);
        }

        public override bool Contains(JsonValue value)
        {
            return this.properties.Values.Contains(value);
        }

        public override void Clear()
        {
            properties.Clear();
        }

        public override JsonValue Clone()
        {
            var clone = new JsonObject();
            foreach (var item in this.properties)
                clone.Add(item.Key, item.Value.Clone());
            return clone;
        } 
        
        public override int GetHashCode()
        {
            return this.properties.GetHashCode();
        }
    }
}