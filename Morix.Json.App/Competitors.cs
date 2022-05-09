using BenchmarkDotNet.Attributes;
using System;

namespace Morix.Json.App
{
    public class Competitors
    {
        private static string json;
        static Competitors()
        {
            json = System.IO.File.ReadAllText("Json.txt");
        }

        [Benchmark]
        public void Morix_Serialize()
        {
            var obj = Morix.Json.JsonConvert.Serialize(DataObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Newton_Serialize()
        {
            var obj = Newtonsoft.Json.JsonConvert.SerializeObject(DataObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Microsoft_Serialize()
        {
            var obj = System.Text.Json.JsonSerializer.Serialize(DataObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Morix_Deserialize()
        {
            var obj = Morix.Json.JsonConvert.Parse(json);
            if (obj == null)
            {

            }
        }

        [Benchmark]
        public void Newton_Deserialize()
        {
            var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
            if (obj == null)
            {

            }
        }

        [Benchmark]
        public void Microsoft_Deserialize()
        {
            var obj = System.Text.Json.JsonDocument.Parse(json);
            if (obj == null)
            {

            }
        }
    }
}