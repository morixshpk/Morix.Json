using System;


namespace Morix.Json.App
{
    public class DataObject
    {
        private static readonly Random _rnd = new Random();

        public int DataInt { get; set; }
        public long DataLong { get; set; }

        public double DataDouble { get; set; }
        public DateTime DataDateTime { get; set; }

        public string DataString { get; set; }

        public static DataObject Create()
        {
            var data = new DataObject
            {
                DataInt = (byte)_rnd.Next(0, int.MaxValue),
                DataLong = (int)_rnd.Next(0, int.MaxValue),
                DataDouble = _rnd.NextDouble(),
                DataString = _rnd.NextDouble().ToString()
            };
            return data;
        }
    }
}