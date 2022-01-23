using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Morix.Json.App
{
    internal class Program
    {
        static void Main()
        {
            string data = "\"hello\nthere\"";
            try
            {
                var morix = JsonConvert.Deserialize<string>(data);
                if (morix != null)
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            try
            {
                var parsed = Newtonsoft.Json.JsonConvert.
                    DeserializeObject<string>(data);
            }
            catch (Exception ex)
            {

            }

            try
            {
                var parsed = System.Text.Json.JsonDocument.Parse(data);

            }
            catch (Exception ex)
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