namespace Morix.Json.Tests
{
    public class JsonIgnoreObject
    {
        public int A;

        [JsonIgnore]
        public int B;

        public int C { get; set; }

        [JsonIgnore]
        public int D { get; set; }
    }
}