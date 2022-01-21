using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonBoolean : JsonValue
    {
        private readonly bool innerValue;

        public override JsonValueType Type
        {
            get { return JsonValueType.Boolean; }
        }

        public override string Value
        {
            get { return (innerValue) ? "true" : "false"; }
            set { }
        }

        public JsonBoolean()
        { }

        public JsonBoolean(bool value)
        {
            this.innerValue = value;
        }

        public override bool ToBolean()
        {
            return innerValue;
        }

        public override bool GetBoolean(string name, bool value = false)
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
        
        public override JsonValue Clone()
        {
            return new JsonBoolean(innerValue);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            else if (obj is JsonBoolean json)
                return this.innerValue.Equals(json.innerValue);
            return false;
        }

        public override int GetHashCode()
        {
            return this.innerValue.GetHashCode();
        }
    }
}