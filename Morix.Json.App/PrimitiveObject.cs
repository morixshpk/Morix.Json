using System;
using System.Collections.Generic;


namespace Morix.Json.App
{
    public class PrimitiveObject
    {
        private static readonly Random _rnd = new Random();

        public bool Bool { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public float Single { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public DateTime Time { get; set; }
        public char Char { get; set; }
        public string String { get; set; }
        public List<int> ListOfInt { get; set; }
        public Dictionary<string, string> DictOfStrings { get; set; }

        public static PrimitiveObject Create()
        {
            var rnd = new PrimitiveObject
            {
                Bool = true,
                Byte = 17,
                SByte = -17,
                Short = -123,
                UShort = 123,
                Int = -56,
                UInt = 56,
                Long = -34,
                ULong = 34,
                Single = 4.3f,
                Double = 5.6,
                Decimal = 10.1M,
                Time = new DateTime(2021, 8, 16),
                Char = 'C',
                String = "String",
                ListOfInt = new List<int> { 1, 2, 3, 4, 5 },
                DictOfStrings = new Dictionary<string, string> { { "1", "2" }, { "3", "4" } }
            };

            rnd.Byte = (byte)_rnd.Next(0, 256);

            rnd.Int = (byte)_rnd.Next(0, int.MaxValue);
            rnd.Long = (byte)_rnd.Next(0, int.MaxValue);

            return rnd;
        }
    }
}
