using System;
using System.Collections.Generic;
using System.Text;

namespace Morix.Json
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonIgnore : Attribute
    {

    }
}
