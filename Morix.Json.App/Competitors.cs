using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.Json;
using BenchmarkDotNet.Engines;

namespace Morix.Json.App
{
    public class Competitors
    {
        private static string jsonText = "";

        public Competitors()
        {
            jsonText = System.IO.File.ReadAllText("Json.txt");
        }

        [Benchmark]
        public void Morix_Serialize()
        {
            var obj = Morix.Json.JsonConvert.Serialize(PrimitiveObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Newton_Serialize()
        {
            var obj = Newtonsoft.Json.JsonConvert.SerializeObject(PrimitiveObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Microsoft_Serialize()
        {
            var obj = System.Text.Json.JsonSerializer.Serialize(PrimitiveObject.Create());
            if (obj == null)
            {
                throw new NullReferenceException();
            }
        }

        [Benchmark]
        public void Morix_Deserialize()
        {
            Morix.Json.JsonConvert.Deserialize<PrimitiveObject>(jsonText);
        }

        [Benchmark]
        public void Newton_Deserialize()
        {
            Newtonsoft.Json.JsonConvert.DeserializeObject<PrimitiveObject>(jsonText);
        }

        [Benchmark]
        public void Microsoft_Deserialize()
        {
            System.Text.Json.JsonSerializer.Deserialize<PrimitiveObject>(jsonText);
        }
    }
}