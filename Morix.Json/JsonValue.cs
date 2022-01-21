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

        public static implicit operator JsonValue(string value)
        {
            return new JsonString(value);
        }

        public virtual bool ToBolean()
        {
            throw new InvalidCastException();
        }

        public virtual byte ToByte()
        {
            throw new InvalidCastException();
        }

        public virtual sbyte ToSByte()
        {
            throw new InvalidCastException();
        }

        public virtual short ToShort()
        {
            throw new InvalidCastException();
        }

        public virtual ushort ToUShort()
        {
            throw new InvalidCastException();
        }

        public virtual int ToInt()
        {
            throw new InvalidCastException();
        }

        public virtual uint ToUInt()
        {
            throw new InvalidCastException();
        }

        public virtual long ToLong()
        {
            throw new InvalidCastException();
        }

        public virtual ulong ToULong()
        {
            throw new InvalidCastException();
        }

        public virtual float ToFloat()
        {
            throw new InvalidCastException();
        }

        public virtual double ToDouble()
        {
            throw new InvalidCastException();
        }

        public virtual decimal ToDecimal()
        {
            throw new InvalidCastException();
        }

        public virtual DateTime ToDateTime()
        {
            throw new InvalidCastException();
        }

        public virtual bool GetBoolean(string name, bool value = default)
        {
            bool result = value;
            try
            {
                var prop = this[name];

                if (prop != null)
                {
                    if (prop.IsBoolean)
                        result = prop.ToBolean();
                }
            }
            catch
            {
                result = value;
            }
            return result;
        }

        public virtual byte GetByte(string name, byte value = default)
        {
            throw new InvalidCastException();
        }

        public virtual SByte GetSByte(string name, SByte value = default)
        {
            throw new InvalidCastException();
        }

        public virtual short GetShort(string name, short value = default)
        {
            throw new InvalidCastException();
        }

        public virtual ushort GetUShort(string name, ushort value = default)
        {
            throw new InvalidCastException();
        }

        public virtual int GetInt(string name, int value = default)
        {
            throw new InvalidCastException();
        }
        
        public virtual uint GetUInt(string name, uint value = default)
        {
            throw new InvalidCastException();
        }

        public virtual long GetLong(string name, long value = default)
        {
            throw new InvalidCastException();
        }

        public virtual ulong GetInt(string name, ulong value = default)
        {
            throw new InvalidCastException();
        }

        public virtual float GetFloat(string name, float value = default)
        {
            throw new InvalidCastException();
        }

        public virtual double GetDouble(string name, double value = default)
        {
            throw new InvalidCastException();
        }

        public virtual decimal GetDecimal(string name, decimal value = default)
        {
            throw new InvalidCastException();
        }

        public virtual DateTime GetDateTime(string name, DateTime value = default)
        {
            throw new InvalidCastException();
        }

        public override string ToString()
        {
            return this.ToJson();
        }

        public string ToJson()
        {
            return JsonWriter.Serialize(this);
        }
    }
}
