using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonString : JsonValue
    {
        private string innerValue;

        public override JsonValueType Type
        {
            get { return JsonValueType.String; }
        }

        public override string Value
        {
            get { return this.innerValue; }
            set
            {
                this.innerValue = value;
            }
        }

        public JsonString()
        { 
        
        }

        public JsonString(string value)
        {
            this.innerValue = value;
        }

        public JsonString(DateTime value)
        {
            this.innerValue = value.ToString("o");
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            else if (obj is JsonString json)
                return this.innerValue.Equals(json.innerValue);
            return false;
        }

        public override int GetHashCode()
        {
            return this.innerValue.GetHashCode();
        }
    }
}
