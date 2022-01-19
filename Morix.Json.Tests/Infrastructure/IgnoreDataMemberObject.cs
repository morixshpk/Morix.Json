using System.Runtime.Serialization;

namespace Morix.Json.Tests
{
    class IgnoreDataMemberObject
    {
        public int A;

        [IgnoreDataMember]
        public int B;

        public int C { get; set; }

        [IgnoreDataMember]
        public int D { get; set; }
    }
}