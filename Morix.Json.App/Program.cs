﻿using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Morix.Json.App
{
    internal class Program
    {
        static void Main()
        {
            var json = new JsonObject
            {
                ["str1"] = "string'string",
                ["str2"] = "2'nd"
            };

            var text = json.ToJson();
            var jobj = JsonConvert.Parse(text);
            var text2 = jobj.ToJson();

            if (text.Equals(text2))
            {

            }
        
            //BenchmarkRunner.Run<Competitors>();

            MyTests();   
        }

        private static void MyTests()
        {
            var s = new Competitors();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            s.Morix_Serialize();
            sw.Stop();
            Console.WriteLine("Morix_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            s.Newton_Serialize();
            sw.Stop();
            Console.WriteLine("Newton_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            s.Microsoft_Serialize();
            sw.Stop();
            Console.WriteLine("Microsoft_Serialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            s.Morix_Deserialize();
            sw.Stop();
            Console.WriteLine("Morix_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            s.Newton_Deserialize();
            sw.Stop();
            Console.WriteLine("Newton_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");
            sw.Restart();
            s.Microsoft_Deserialize();
            sw.Stop();
            Console.WriteLine("Microsoft_Deserialize ".PadRight(30) + sw.ElapsedMilliseconds.ToString().PadLeft(5) + " ms");

            Console.ReadKey();
        }
    }
}