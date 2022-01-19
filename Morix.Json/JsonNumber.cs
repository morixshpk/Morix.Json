using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonNumber : JsonValue
    {
        public string innerValue;

        public override JsonValueType Type
        {
            get { return JsonValueType.Number; }
        }

        public override string Value
        {
            get { return innerValue; }
            set { this.innerValue = value; }
        }
       
        public JsonNumber(string value)
        {
            if (value != null)
            {
                this.innerValue = value;
            }
        }

        public JsonNumber(sbyte? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(short? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(ushort? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(int? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(uint? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(long? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(ulong? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString();
            }
        }

        public JsonNumber(float? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public JsonNumber(double? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public JsonNumber(decimal? value)
        {
            if (value.HasValue)
            {
                this.innerValue = value.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public override JsonValue Clone()
        {
            return new JsonNumber(this.innerValue);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            else if (obj is JsonNumber json)
                return this.innerValue.Equals(json.innerValue);
            return false;
        }

        public override int GetHashCode()
        {
            return this.innerValue.GetHashCode();
        }

        public override byte ToByte()
        {
            return byte.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override sbyte ToSByte()
        {
            return sbyte.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override short ToShort()
        {
            return short.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override ushort ToUShort()
        {
            return ushort.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override int ToInt()
        {
            return int.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override uint ToUInt()
        {
            return uint.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override long ToLong()
        {
            return long.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override ulong ToULong()
        {
            return ulong.Parse(this.innerValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }

        public override float ToFloat()
        {
            return float.Parse(this.innerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public override double ToDouble()
        {
            return double.Parse(this.innerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public override decimal ToDecimal()
        {
            return decimal.Parse(this.innerValue, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
    }
}
