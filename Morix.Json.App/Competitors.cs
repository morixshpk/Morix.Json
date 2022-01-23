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
        [Benchmark]
        public void Morix_Serialize()
        {
            var data = DataObject.Create();
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
            var jsonText = GetText();
            Morix.Json.JsonConvert.Deserialize<DataObject>(jsonText);
        }

        [Benchmark]
        public void Newton_Deserialize()
        {
            var jsonText = GetText();
            Newtonsoft.Json.JsonConvert.DeserializeObject<DataObject>(jsonText);
        }

        [Benchmark]
        public void Microsoft_Deserialize()
        {
            var jsonText = GetText();
            System.Text.Json.JsonSerializer.Deserialize<DataObject>(jsonText);
        }

        private static string GetText()
        {
            var data = DataObject.Create();
            return JsonConvert.Serialize(data);
        }
    }
}