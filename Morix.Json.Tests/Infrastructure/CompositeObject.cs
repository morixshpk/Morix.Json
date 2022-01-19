using System.Collections.Generic;

namespace Morix.Json.Tests
{
    public class CompositeObject
    {
        public int A;
        public int B;
        public CompositeObject C;
        public List<CompositeObject> D;
        public AStructWithClassMember E;
    }
}
