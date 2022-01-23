using System;
using System.Collections.Generic;


namespace Morix.Json.App
{
    public class DataObject
    {
        private static readonly Random _rnd = new Random();

        public int Int { get; set; }
        public long Long { get; set; }

        public double Double { get; set; }
        public DateTime Time { get; set; }
 
        public string String { get; set; }

        public static DataObject Create()
        {
            var data = new DataObject
            {
                Int = (byte)_rnd.Next(0, int.MaxValue),
                Long = (int)_rnd.Next(0, int.MaxValue),
                Double = _rnd.NextDouble(),
                String = _rnd.NextDouble().ToString()
            };
            return data;
        }
    }
}