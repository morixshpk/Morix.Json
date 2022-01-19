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

        public override double ToNumberDouble()
        {
            return double.Parse(this.innerValue, NumberStyles.Float, CultureInfo.InvariantCulture);    
        }
    }
}
