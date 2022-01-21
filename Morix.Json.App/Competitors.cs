using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.Json;

namespace Morix.Json.App
{
    public class Competitors
    {
        [Benchmark]
        public void Morix_Serialize()
        {
            for (int i = 0; i < 100; i++)
            {
                Morix.Json.JsonConvert.Serialize(PrimitiveObject.Create());
            }
        }

        [Benchmark]
        public void Newton_Serialize()
        {
            for (int i = 0; i < 100; i++)
            {
                Newtonsoft.Json.JsonConvert.SerializeObject(PrimitiveObject.Create());
            }
        }

        [Benchmark]
        public void Microsoft_Serialize()
        {
            for (int i = 0; i < 100; i++)
            {
                JsonSerializer.Serialize(PrimitiveObject.Create());
            }
        }

        [Benchmark]
        public void Morix_Deserialize()
        {
            var json = System.IO.File.ReadAllText("Json.txt");
            for (int i = 0; i < 10; i++)
            {
                Morix.Json.JsonConvert.Deserialize<PrimitiveObject>(json);
            }
        }

        [Benchmark]
        public void Newton_Deserialize()
        {
            var json = System.IO.File.ReadAllText("Json.txt");
            for (int i = 0; i < 10; i++)
            {
                Newtonsoft.Json.JsonConvert.DeserializeObject<PrimitiveObject>(json);
            }
        }

        [Benchmark]
        public void Microsoft_Deserialize()
        {
            var json = System.IO.File.ReadAllText("Json.txt");
            for (int i = 0; i < 10; i++)
            {
                System.Text.Json.JsonSerializer.Deserialize<PrimitiveObject>(json);
            }
        }
    }
}