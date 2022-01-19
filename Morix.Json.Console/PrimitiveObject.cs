using System;
using System.Collections.Generic;


namespace Morix.Json.Console
{
    public class PrimitiveObject
    {
        private static Random _rnd = new Random();

        public bool Bool;
        public byte Byte;
        public sbyte SByte;
        public short Short;
        public ushort UShort;
        public int Int;
        public uint UInt;
        public long Long;
        public ulong ULong;
        public float Single;
        public double Double;
        public decimal Decimal;
        public DateTime Time;
        public char Char;
        public string String;
        public List<int> ListOfInt;
        public Dictionary<string, string> DictOfStrings;

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
