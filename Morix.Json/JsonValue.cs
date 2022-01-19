using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public abstract class JsonValue
    {
        public bool IsNull { get { return this.Type == JsonValueType.Null; } }

        public bool IsBoolean { get { return this.Type == JsonValueType.Boolean; } }

        public bool IsNumber { get { return this.Type == JsonValueType.Number; } }

        public bool IsString { get { return this.Type == JsonValueType.String; } }

        public bool IsArray { get { return this.Type == JsonValueType.Array; } }

        public bool IsObject { get { return this.Type == JsonValueType.Object; } }

        public JsonValue()
        { }

        public virtual JsonValueType Type
        {
            get { return JsonValueType.Null; }
        }

        public virtual string Value { get; set; }

        public virtual int Count { get { return 0; } }

        public virtual JsonValue this[int index] { get { return null; } set { } }

        public virtual JsonValue this[string key] { get { return null; } set { } }

        public virtual bool ContainsKey(string key)
        {
            return false;
        }

        public virtual bool Contains(JsonValue value)
        {
            return false;
        }

        public virtual void Clear()
        {
        }

        public virtual JsonValue Add(JsonValue jsonValue)
        {
            return this;
        }

        public virtual JsonValue Clone()
        {
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return this.IsNull;
            }

            JsonValue jsonValue = obj as JsonValue;

            if (jsonValue != null)
            {
                return this.Type == jsonValue.Type && this.Value == jsonValue.Value;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool pretty)
        {
            return JsonWriter.Serialize(this, pretty);
        }

        public static implicit operator JsonValue(string value)
        {
            return new JsonString(value);
        }

        public virtual double ToNumberDouble()
        { 
            throw new InvalidCastException();
        }
    }
}
