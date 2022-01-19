using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morix.Json
{
    public class JsonNull : JsonValue
    {
        public static JsonNull Null = new JsonNull();

        public override JsonValueType Type
        {
            get { return JsonValueType.Null; }
        }

        public override string Value
        {
            get { return "null"; }
            set { }
        }

        public override JsonValue Clone()
        {
            return Null;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            return (obj is JsonNull);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
