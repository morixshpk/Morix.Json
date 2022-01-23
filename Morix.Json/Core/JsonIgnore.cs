using System;

namespace Morix.Json
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonIgnore : Attribute
    {

    }
}
