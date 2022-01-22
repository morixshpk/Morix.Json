using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonValue
    {
        private readonly string InnerValue;

        public bool IsNull { get { return Type == JsonType.Null; } }

        public bool IsBoolean { get { return Type == JsonType.Boolean; } }

        public bool IsNumber { get { return Type == JsonType.Number; } }

        public bool IsString { get { return Type == JsonType.String; } }

        public bool IsArray { get { return Type == JsonType.Array; } }

        public bool IsObject { get { return Type == JsonType.Object; } }

        public JsonValue()
        {
        }

        public JsonValue(JsonType type)
        {
            Type = type;
        }

        public JsonValue(JsonType type, string value)
        {
            Type = type;
            InnerValue = value;
        }

        public JsonValue(bool value)
        {
            Type = JsonType.Boolean;
            InnerValue = (value) ? "1" : "0";
        }

        public JsonValue(short? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(ushort? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(int? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(uint? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(long? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(ulong? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(float? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public JsonValue(double? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public JsonValue(decimal? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public JsonValue(string value)
        {
            Type = JsonType.String;
            InnerValue = value;
        }

        public JsonValue(sbyte? value)
        {
            if (value.HasValue)
            {
                Type = JsonType.Number;
                InnerValue = value.Value.ToString();
            }
        }

        public JsonValue(DateTime value)
        {
            Type = JsonType.String;
            InnerValue = value.ToString("o");
        }

        public JsonValue(Guid value)
        {
            Type = JsonType.String;
            InnerValue=value.ToString();
        }

        public JsonType Type { get; private set; }

        public virtual string Value
        {
            get
            {
                switch (Type)
                {
                    case JsonType.Null:
                        return "null";
                    case JsonType.Boolean:
                        if (this.InnerValue == "1")
                            return "true";
                        else
                            return "false";
                    case JsonType.Number:
                    case JsonType.String:
                        return InnerValue;
                    default:
                        throw new InvalidOperationException("No value found for this type");
                }
            }
        }

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
                return this.IsNull;

            if (obj is JsonValue jsonValue)
            {
                return Type == jsonValue.Type && this.Value == jsonValue.Value;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            if (Type == JsonType.Boolean)
                return InnerValue.GetHashCode();
            else if (Type == JsonType.Number)
                return InnerValue.GetHashCode();
            else if (Type == JsonType.String)
                return InnerValue.GetHashCode();

            return 0;
        }

        public virtual bool ToBolean()
        {
            if (this.IsBoolean)
                return InnerValue.Equals("1");
            throw new InvalidCastException();
        }

        public byte ToByte()
        {
            if (IsNumber)
                return byte.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public sbyte ToSByte()
        {
            if (IsNumber)
                return sbyte.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public short ToShort()
        {
            if (IsNumber)
                return short.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public ushort ToUShort()
        {
            if (IsNumber)
                return ushort.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public int ToInt()
        {
            if (IsNumber)
                return int.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public uint ToUInt()
        {
            if (IsNumber)
                return uint.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public long ToLong()
        {
            if (IsNumber)
                return long.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public ulong ToULong()
        {
            if (IsNumber)
                return ulong.Parse(this.InnerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public float ToFloat()
        {
            if (IsNumber)
                return float.Parse(this.InnerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public double ToDouble()
        {
            if (IsNumber)
                return double.Parse(this.InnerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public decimal ToDecimal()
        {
            if (this.IsNumber)
                return decimal.Parse(this.InnerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public DateTime ToDateTime()
        {
            if (Type == JsonType.String)
                return DateTime.ParseExact(this.InnerValue, "o", CultureInfo.InvariantCulture);
            throw new InvalidCastException();
        }

        public Guid ToGuid()
        {
            if (Type == JsonType.String)
                return Guid.Parse(this.InnerValue);
            throw new InvalidCastException();
        }

        public bool GetBoolean(string name, bool value = default)
        {
            if (this.IsObject)
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
            throw new InvalidCastException();
        }

        public byte GetByte(string name, byte value = default)
        {
            if (this.IsObject)
            {

                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToByte();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public SByte GetSByte(string name, SByte value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToSByte();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public short GetShort(string name, short value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToShort();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public ushort GetUShort(string name, ushort value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToUShort();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public int GetInt(string name, int value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToInt();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }
        
        public uint GetUInt(string name, uint value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToUInt();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public long GetLong(string name, long value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToLong();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public ulong GetULong(string name, ulong value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToULong();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public float GetFloat(string name, float value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToFloat();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public double GetDouble(string name, double value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToDouble();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public decimal GetDecimal(string name, decimal value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsNumber)
                    {
                        return prop.ToDecimal();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public string GetString(string name, string value = "")
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsString)
                    {
                        return prop.Value;
                    }
                }
                catch
                {

                }
                return value;
            }
            throw new InvalidCastException();
        }

        public DateTime GetDateTime(string name, DateTime value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsString)
                    {
                        return prop.ToDateTime();
                    }
                }
                catch
                {
                }
                return value;
            }
            throw new InvalidCastException();
        }

        public Guid GetGuid(string name, Guid value = default)
        {
            if (this.IsObject)
            {
                try
                {
                    var prop = this[name];

                    if (prop != null && prop.IsString)
                    {
                        return prop.ToGuid();
                    }
                }
                catch
                {
                }
                return value;
            }
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


        //Static members
        public static JsonValue Null = new JsonValue();

        public static implicit operator JsonValue(bool value)
        { 
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(byte value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(sbyte value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(short value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(ushort value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(int value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(uint value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(long value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(ulong value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(float value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(double value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(decimal value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(string value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(DateTime value)
        {
            return new JsonValue(value);
        }
        public static implicit operator JsonValue(Guid value)
        {
            return new JsonValue(JsonType.String, value.ToString());
        }
    }
}
