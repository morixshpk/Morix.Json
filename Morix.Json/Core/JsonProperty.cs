using System;
using System.Collections.Generic;
using System.Text;

namespace Morix.Json
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonProperty : Attribute
    {
        public JsonProperty()
        {

        }

        public JsonProperty(string name)
        { 
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
