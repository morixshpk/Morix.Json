namespace Morix.Json.Tests
{
    struct PriceStruct
    {
        public int Value;

        public override string ToString()
        {
            if (Value < int.MinValue)
                Value = 0;

            return this.Value.ToString();
        }
    }
}