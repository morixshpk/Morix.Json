
namespace Morix.Json.Tests
{
    public class JsonPropertyObject
    {
        [JsonProperty("a")]
        public int A;

        public int B;

        [JsonProperty("c")]
        public int C { get; set; }

        public int D { get; set; }
    }
}