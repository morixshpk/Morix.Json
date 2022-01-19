using System.Runtime.Serialization;

namespace Morix.Json.Tests
{
    public class DataMemberObject
    {
        [DataMember(Name = "a")]
        public int A;

        [DataMember()]
        public int B;

        [DataMember(Name = "c")]
        public int C { get; set; }
        
        public int D { get; set; }
    }
}
