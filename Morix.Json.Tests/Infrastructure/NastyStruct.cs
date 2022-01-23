namespace Morix.Json.Tests
{
    public struct NastyStruct
    {
        public byte R, G, B;
        public NastyStruct(byte r, byte g, byte b)
        {
            R = r; G = g; B = b;
        }

        public static NastyStruct Nasty = new NastyStruct(0, 0, 0);
    }
}
