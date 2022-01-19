using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


namespace Morix.Json.Console
{
    public class Competitors
    {
        [Benchmark]
        public void NewtonJsonParse()
        {
            var json = System.IO.File.ReadAllText("Json.txt");
            for (int i = 0; i < 100; i++)
            {
                var obj = Newtonsoft.Json.Linq.JObject.Parse(json);
            }
        }

        [Benchmark]
        public  void MorixJsonParse()
        {
            var json = System.IO.File.ReadAllText("Json.txt");
            for (int i = 0; i < 100; i++)
            {
                var obj = Morix.Json.JsonConvert.Parse(json);

            }
        }

        [Benchmark]
        public void NewtonJsonSerialize()
        {
            for (int i = 0; i < 1000; i++)
            {
                Newtonsoft.Json.JsonConvert.SerializeObject(PrimitiveObject.Create());
            }
        }

        [Benchmark]
        public void MorixJsonSerialize()
        {
            for (int i = 0; i < 1000; i++)
            {
                Morix.Json.JsonConvert.Serialize(PrimitiveObject.Create());
            }
        }
    }
}